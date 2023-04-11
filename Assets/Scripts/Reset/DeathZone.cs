using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IResetable resetObj = other.attachedRigidbody.GetComponent<IResetable>();
        if (resetObj != null)
        {
            resetObj.Reset();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        IResetable resetObj = collision.rigidbody.GetComponent<IResetable>();
        if (resetObj != null)
        {
            resetObj.Reset();
        }
    }
}
