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
        rb.velocity += Vector3.up * bulletInfo.selfGravity * Time.fixedDeltaTime;
    }

    #endregion

    #region Public Custom Methods

    #endregion

    #region Private Custom Methods

    public void FireBullet(BulletInfo bulletInfo)
    {
        this.bulletInfo = bulletInfo;
        rb.velocity = bulletInfo.vel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf)
            return;
        Debug.Log(gameObject + " has hit: " + other.gameObject);
        ObjectPool.instance.ReturnObject(selfPoolableObject);
    }

    #endregion
}

public interface ITankBullet
{
    public void FireBullet(BulletInfo bulletInfo);
}

public struct BulletInfo
{
    public Vector3 vel;
    public float selfGravity;
}