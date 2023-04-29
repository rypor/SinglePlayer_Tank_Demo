using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(IPoolableObject))]
public class StandardTankBullet : MonoBehaviour, ITankBullet
{
    #region Properties

    private BulletInfo bulletInfo;
    private IPoolableObject selfPoolableObject;
    [SerializeField] private Rigidbody rb;

    #endregion

    #region Built-in Methods

    private void Awake()
    {
        selfPoolableObject = GetComponent<IPoolableObject>();
    }

    private void FixedUpdate()
    {
        rb.velocity += Vector3.up * bulletInfo.SelfGravity * Time.deltaTime;
    }

    #endregion

    #region Public Custom Methods

    #endregion

    #region Private Custom Methods

    public void FireBullet(BulletInfo bulletInfo)
    {
        this.bulletInfo = bulletInfo;
        rb.velocity = bulletInfo.Vel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf)
            return;

        ExplosionManager.instance.SpawnExplosion(rb.position, bulletInfo.ExplosionRange, bulletInfo.ExplosionPower, bulletInfo.ExplosionUpwardsModifier);
        AudioManager.instance.PlaySoundAtPoint(AudioTypeEnum.StandardExplosion, rb.position);
        CameraManager.instance.StartScreenShake(bulletInfo.ExplosionScreenShake_Duration, bulletInfo.ExplosionScreenShake_Intensity);
        PaintManager.instance.PaintSphere(rb.position, bulletInfo.ExplosionRange * bulletInfo.PaintSizeModifier, 0.4f, new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));
        selfPoolableObject.DisableObject();
    }

    #endregion
}

public interface ITankBullet
{
    public void FireBullet(BulletInfo bulletInfo);
}

public struct BulletInfo
{
    public Vector3 Vel;
    public float SelfGravity;

    public float ExplosionRange;
    public float ExplosionPower;
    public float ExplosionUpwardsModifier;

    public float ExplosionScreenShake_Duration;
    public float ExplosionScreenShake_Intensity;

    public float PaintSizeModifier;
}