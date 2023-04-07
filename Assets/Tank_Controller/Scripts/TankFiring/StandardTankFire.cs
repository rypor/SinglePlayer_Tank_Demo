using INoodleI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;

[RequireComponent(typeof(TankInput))]
public class StandardTankFire : MonoBehaviour
{
    #region Properties

    [Header("Gun References")]
    [SerializeField] private Transform GunPivotHorizontal;
    [SerializeField] private Transform GunPivotVertical;
    [SerializeField] private Transform GunFirePoint;
    private Transform reticle;

    [Header("Stats")]
    [SerializeField] private StandardTankFireStats stats;

    private TankInput input;
    private InputData inputData;

    private float _targetGunAngle;
    private Vector3 _targetLookDir;
    private Vector3 _target;

    private bool _bufferFire;

    #endregion

    #region Built-in Methods

    private void Start()
    {
        input = GetComponent<TankInput>();

        reticle = ObjectPool.instance.RequestObject(stats.ReticleEnum, Vector3.zero, Quaternion.identity, true).GameObject().transform;
    }

    private void Update()
    {
        if (input)
        {
            inputData = input.InputData;

            if (inputData.FirePressed)
                _bufferFire = true;
        }
    }

    private void FixedUpdate()
    {
        if (reticle)
        {
            UpdateGun();
            FireGun();
        }
    }

    private void UpdateGun()
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
    }

    private void FireGun()
    {
        Vector3 vertRot = CalculateBulletTrajectory(GunFirePoint.position);
        UpdateGunVert(vertRot);

        Vector3 vel = CalculateBulletTrajectory(GunFirePoint.position);
        if (_bufferFire)
        {
            ITankBullet bullet = ObjectPool.instance.RequestObject(stats.BulletEnum, GunFirePoint.position, GunFirePoint.rotation, true).GameObject().GetComponent<ITankBullet>();
            BulletInfo info = new BulletInfo() { vel = vel, selfGravity = stats.SelfGravity};
            bullet.FireBullet(info);
            _bufferFire = false;
        }
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
        velocity.y = bulletYVel;

        return velocity;
    }

    #endregion
}
