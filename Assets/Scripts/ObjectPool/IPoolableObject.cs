using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    public PrefabEnum PrefabEnumValue { get; set; }
    public bool Active { get; }

    public GameObject GameObject();
    
    public IPoolableObject EnableObject(Vector3 pos, Quaternion rot);
    public IPoolableObject DisableObject();

    public IPoolableObject DelayedDisableObject(float time);

}
