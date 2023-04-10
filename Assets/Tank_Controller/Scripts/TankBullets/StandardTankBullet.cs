using System.Collections;
using System.Collections.Generic;
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
        Debug.Log(gameObject + " has hit: " + other.gameObject);
        ExplosionManager.instance.SpawnExplosion(rb.position, bulletInfo.ExplosionRange, bulletInfo.ExplosionPower);
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
}