using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleRBReset : MonoBehaviour, IResetable
{
    [SerializeField] private Transform _resetLocation;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        _rb.position = _resetLocation.position;
        _rb.rotation = _resetLocation.rotation;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
