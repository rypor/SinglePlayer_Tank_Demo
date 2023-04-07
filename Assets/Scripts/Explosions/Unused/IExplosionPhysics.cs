using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosionPhysics
{
    public void ReactToExplosion(Vector3 pos, float force);
}
