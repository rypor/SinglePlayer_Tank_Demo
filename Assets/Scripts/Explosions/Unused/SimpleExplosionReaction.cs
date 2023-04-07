using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class SimpleExplosionReaction : MonoBehaviour, IExplosionPhysics
{
    #region Properties

    private Rigidbody rb;

    #endregion


    #region Built-in Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    #endregion


    #region IExplosionPhysics Methods

    public void ReactToExplosion(Vector3 pos, float force)
    {
        
    }

    #endregion
}
