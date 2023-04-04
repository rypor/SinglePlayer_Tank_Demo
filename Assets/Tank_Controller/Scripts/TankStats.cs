using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankStats", menuName ="Tank/Stats")]
public class TankStats : ScriptableObject
{
    [Header("LayerMasks")]
    public LayerMask GroundMask;

    [Header("Movement")]
    [Tooltip("Center of Mass offset for RB. Lower is better for more tank stability. Identifiable by red wire sphere gizmo")]
    public Vector3 CenterOfMass;

    public float InputDeadzone = 0.1f;

    public float MaxSpeed = 10f;
    public float Acceleration = 2f;
    public float Decceleration = 2f;

    public float RotMaxSpeed = 180f;
    public float RotAcceleration = 4f;
    public float RotDecceleration = 5f;

    [Header("Firing")]
    public float MaxGunRange = 15f;
    public float GunRotateSpeed = 3f;

    public float FiringDelay = 0.7f;

    [Header("Grounding")]
    public Vector3 GroundingCheckSize;

}
