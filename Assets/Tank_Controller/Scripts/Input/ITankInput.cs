using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankInput
{
    public InputData InputData { get; }
}
public struct InputData
{
    public Vector2 Movement;
    public bool FirePressed;
    public bool MinePressed;

    public bool ReticleHit;
    public Vector3 ReticlePosition;
    public Vector3 ReticleNormal;
}