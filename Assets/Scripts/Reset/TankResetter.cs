using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITankController), typeof(Rigidbody))]
public class TankResetter : MonoBehaviour, IResetable
{
    [SerializeField] private Transform _resetLocation;
    private ITankController _controller;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<ITankController>();
        _rb = GetComponent<Rigidbody>();
    }
    public void Reset()
    {
        _rb.position = _resetLocation.position;
        _rb.rotation = _resetLocation.rotation;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _controller.StopMotion();
    }
}
