using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPoolableObject))]
public class StandardTankBullet : MonoBehaviour, ITankBullet
{
    #region Properties

    [SerializeField] private float flatSpeed;
    [SerializeField] private float selfGravity;

    private BulletInfo bulletInfo;
    private IPoolableObject selfPoolableObject;
    [SerializeField] private Rigidbody rb;

    #endregion

    #region Built-in Methods

    private void Awake()
    {
        selfPoolableObject = GetComponent<IPoolableObject>();
    }

    #endregion

    #region Public Custom Methods

    public void FireBullet(BulletInfo bulletInfo)
    {
        this.bulletInfo = bulletInfo;
        CalculateBulletTrajectory();
    }

    #endregion

    #region Private Custom Methods

    private void CalculateBulletTrajectory()
    {
        Vector3 dir = bulletInfo.BulletTarget - rb.position;
        dir.y = 0;

        float flatDist = dir.magnitude;
        float travelTime = flatDist / flatSpeed;

        // Calculate Initial Y Velocity for the bullet
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject + " has hit: " + other.gameObject);
        ObjectPool.instance.ReturnObject(selfPoolableObject);
    }

    #endregion
}

public interface ITankBullet
{
    public void FireBullet(BulletInfo bulletStats);
}

public class BulletInfo
{
    public Vector3 BulletTarget; 

    public float BulletSpeedModifier;

    public float ExplosionRange;
    public float ExplosionPower;
}