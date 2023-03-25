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

    public float MaxSpeed;
    public float Acceleration;
    public float Decceleration;

    public float RotMaxSpeed;
    public float RotAcceleration;
    public float RotDecceleration;

    [Header("Grounding")]
    public Vector3 GroundingCheckSize;

}
