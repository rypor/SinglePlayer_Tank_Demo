using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneTank : MonoBehaviour, ITankInput
{
    private InputData _data;
    public InputData InputData => _data;

    [SerializeField] private List<Transform> _lookAtPoints;
    [SerializeField] private Vector2 _minMaxLookDelay = new Vector2(4, 10);
    [SerializeField] private float _targetLerpSpeed = 1f;
    [SerializeField] private float _fireFrequency = 1f;

    private Vector3 _target;
    private int _selectedIndex = 0;
    private float _timer;

    private void Start()
    {
        _data = new InputData
        {
            Movement = Vector3.zero,
            FirePressed = false,
            MinePressed = false,
            ReticleHit = true,
            ReticlePosition = Vector3.zero,
            ReticleNormal = Vector3.up,
        };

        _timer = Random.Range(_minMaxLookDelay.x, _minMaxLookDelay.y);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            ChooseNewLookAtPoint();
            _timer = Random.Range(_minMaxLookDelay.x, _minMaxLookDelay.y);
        }


        _target = Vector3.Lerp(_target, _lookAtPoints[_selectedIndex].position, _targetLerpSpeed * Time.deltaTime);

        _data.FirePressed = Random.Range(0f, 100f) <= _fireFrequency;
        _data.ReticlePosition = _target;
    }

    private void ChooseNewLookAtPoint()
    {
        Debug.Log(_lookAtPoints.Count + ",  s: "+_selectedIndex);
        if(_lookAtPoints.Count > 1)
        {
            int newIndex = _selectedIndex;
            while(newIndex == _selectedIndex)
            {
                newIndex = Random.Range(0, _lookAtPoints.Count);
            }
            _selectedIndex = newIndex;
            Debug.Log("s: " + _selectedIndex);
        }
    }
}
