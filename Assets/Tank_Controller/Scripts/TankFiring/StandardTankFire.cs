using INoodleI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;

[RequireComponent(typeof(ITankInput))]
public class StandardTankFire : MonoBehaviour
{
    #region Properties

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Gun References")]
    [SerializeField] private Transform GunPivotHorizontal;
    [SerializeField] private Transform GunPivotVertical;
    [SerializeField] private Transform GunFirePoint;
    private Transform reticle;

    [Header("Stats")]
    [SerializeField] private StandardTankFireStats stats;

    private ITankInput input;
    private InputData inputData;

    private float _targetGunAngle;
    private Vector3 _targetLookDir;
    private Vector3 _target;

    private bool _bufferFire;
    private bool _hasInputClass = false;

    private int _numShotsFiredSinceDelay;

    private float _fireDelayTimer;
    private float _fireReloadTimer;
    private float _bufferTimer;
    private bool CanFire => _bufferFire && (_fireDelayTimer <= 0) && (_numShotsFiredSinceDelay < stats.ShotsBeforeForceReload);
    private bool BufferedFire => _bufferFire && (_bufferTimer > 0);

    #endregion

    #region Built-in Methods

    private void Start()
    {
        input = GetComponent<ITankInput>();
        _hasInputClass = true;

        reticle = ObjectPool.instance.RequestObject(stats.ReticleEnum, Vector3.zero, Quaternion.identity, true).GameObject().transform;
        _fireDelayTimer = 0;
        _numShotsFiredSinceDelay = 0;
    }

    private void Update()
    {
        if (_hasInputClass)
        {
            inputData = input.InputData;

            if (inputData.FirePressed)
            {
                _bufferFire = true;
                _bufferTimer = stats.FiringBufferTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (reticle)
        {
            UpdateGunPos();
            if(GameManager.instance.IsGamePlaying())
                HandleFire();
        }
    }

    private void UpdateGunPos()
    {
        if (inputData.ReticleHit)
        {
            // Adjust Reticle Position
            //reticle.gameObject.SetActive(true);
            reticle.position = inputData.ReticlePosition;
            reticle.up = inputData.ReticleNormal;

            // Update Gun Target Position to new Reticle direction
            _targetLookDir = (reticle.position - GunPivotHorizontal.position);
            _targetLookDir.y = 0;

            _target = inputData.ReticlePosition;
        }
        else
        {
            //reticle.gameObject.SetActive(false);
        }

        Quaternion lookRotation = Quaternion.LookRotation(_targetLookDir, Vector3.up);
        GunPivotHorizontal.rotation = Quaternion.Lerp(GunPivotHorizontal.rotation, lookRotation, stats.GunRotateSpeed * Time.fixedDeltaTime);

        Vector3 vertRot = CalculateBulletTrajectory(GunFirePoint.position);
        UpdateGunVert(vertRot);
    }

    private void HandleFire()
    {
        if (BufferedFire && CanFire)
        {
            FireGun();
            _bufferFire = false;
            _fireReloadTimer = stats.FiringReloadTime;
            _fireDelayTimer = stats.FiringDelayTime;
            _numShotsFiredSinceDelay++;
        }
        _bufferTimer -= Time.fixedDeltaTime;
        _fireReloadTimer -= Time.fixedDeltaTime;
        _fireDelayTimer -= Time.fixedDeltaTime;
        if(_fireReloadTimer <= 0)
            Reload();
    }
    private void Reload()
    {
        _numShotsFiredSinceDelay = 0;
    }

    private void FireGun()
    {
        Vector3 vel = CalculateBulletTrajectory(GunFirePoint.position);
        ITankBullet bullet = ObjectPool.instance.RequestObject(stats.BulletEnum, GunFirePoint.position, GunFirePoint.rotation, true).GameObject().GetComponent<ITankBullet>();
        BulletInfo info = new BulletInfo() 
        { 
            Vel = vel, 
            SelfGravity = stats.SelfGravity, 
            ExplosionRange = stats.ExplosionRange, 
            ExplosionPower = stats.ExplosionPower, 
            ExplosionUpwardsModifier = stats.ExplosionUpwardsModifer,
            ExplosionScreenShake_Duration = stats.ExplosionScreenShake_Duration, 
            ExplosionScreenShake_Intensity = stats.ExplosionScreenShake_Intensity, 
            PaintSizeModifier = stats.PaintSizeModifier,
        };
        bullet.FireBullet(info);

        AudioManager.instance.PlaySoundAtPoint(AudioTypeEnum.StandardTankFire, GunFirePoint.position);
        CameraManager.instance.StartScreenShake(stats.FiringScreenShake_Duration, stats.FiringScreenShake_Intensity);
        _animator.Play("Fire");
    }

    private void UpdateGunVert(Vector3 vel)
    {
        float riseOverRun = vel.y / new Vector2(vel.x, vel.z).magnitude;
        float targetAngle = Mathf.Clamp(-Mathf.Atan(riseOverRun) * 180/Mathf.PI, -stats.MaxVertAngle, stats.MaxVertAngle);
        GunPivotVertical.localRotation = Quaternion.Lerp(GunPivotVertical.localRotation, Quaternion.Euler(new Vector3(targetAngle, 0, 0)), stats.GunRotateSpeed * Time.fixedDeltaTime);
    }
    private Vector3 CalculateBulletTrajectory(Vector3 startPoint)
    {
        Vector3 dir = _target - startPoint;
        dir.y = 0;

        float flatDist = dir.magnitude;
        float travelTime = flatDist / stats.FlatSpeed;

        // Calculate Initial Y Velocity for the bullet
        // y=y_0 + v_0*t + 0.5*g*t^2
        // y - y_0 - 0.5*g*t^2 = v_0*t
        // ( y - y_0 - 0.5 * g * t^2 )/t = v_0

        //float bulletYVel = (bulletInfo.BulletTarget.y - rb.position.y - 0.5f * selfGravity * travelTime * travelTime) / travelTime;
        float bulletYVel = _target.y - startPoint.y;
        bulletYVel -= 0.5f * stats.SelfGravity * travelTime * travelTime;
        bulletYVel = bulletYVel / travelTime;

        Vector3 velocity = dir.normalized * stats.FlatSpeed;
        velocity.y = Mathf.Clamp(bulletYVel, -stats.MaxBulletYSpeed, stats.MaxBulletYSpeed);

        return velocity;
    }

    #endregion
}
