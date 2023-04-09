using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankStats", menuName ="Tank/TankStats")]
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

    [Header("Grounding")]
    public Vector3 GroundingCheckSize;
    public float MaxGroundAngle = 35;
    public float MaxFullSpeedGroundAngle = 25;

    [Header("Slopes")]
    public float MinSlopeAngle = 15f;
    public float MaxSlopeAngle = 45f;
    public float SlopeGravityModifier = 1f;
}
