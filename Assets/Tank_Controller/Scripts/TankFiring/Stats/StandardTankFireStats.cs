using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StandardFireStats", menuName = "Tank/Fire Stats/Standard Fire")]
public class StandardTankFireStats : ScriptableObject
{
    [Header("Prefabs")]
    public PrefabEnum BulletEnum = PrefabEnum.StandardTankBullet;
    public PrefabEnum ReticleEnum = PrefabEnum.StandardTankBullet;

    [Header("Firing")]
    public float MaxVertAngle = 75f;
    public float MaxGunRange = 15f;
    public float GunRotateSpeed = 3f;

    public float FiringDelay = 0.7f;

    [Header("Bullet Properties")]
    public float FlatSpeed = 6;
    public float SelfGravity = -10;
    public float MaxReasonableLife = 30;

    [Header("Explosion Properties")]
    public float ExplosionRange = 10f;
    public float ExplosionPower = 20f;
}
