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

    private void FixedUpdate()
    {
        rb.velocity += Vector3.up * selfGravity * Time.fixedDeltaTime;
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
        Debug.Log(rb.position);

        float flatDist = dir.magnitude;
        float travelTime = flatDist / flatSpeed;
        Debug.Log(dir + ", " + travelTime);

        // Calculate Initial Y Velocity for the bullet
        // y=y_0 + v_0*t + 0.5*g*t^2
        // y - y_0 - 0.5*g*t^2 = v_0*t
        // ( y - y_0 - 0.5 * g * t^2 )/t = v_0

        //float bulletYVel = (bulletInfo.BulletTarget.y - rb.position.y - 0.5f * selfGravity * travelTime * travelTime) / travelTime;
        float bulletYVel = bulletInfo.BulletTarget.y - rb.position.y;
        bulletYVel -= 0.5f * selfGravity * travelTime * travelTime;
        bulletYVel = bulletYVel / travelTime;
        Debug.Log("Firing Bullet with :" + bulletYVel + " y vel.");

        Vector3 velocity = dir.normalized * flatSpeed;
        velocity.y = bulletYVel;
        rb.velocity = velocity;
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

public struct BulletInfo
{
    public Vector3 BulletTarget; 
}