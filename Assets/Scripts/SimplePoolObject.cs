using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePoolObject : MonoBehaviour, IPoolableObject
{

    #region Properties

    private PrefabEnum prefabEnumValue;

    #endregion


    #region IPoolableObject Methods

    public PrefabEnum PrefabEnumValue { get => prefabEnumValue; set => prefabEnumValue = value; }

    public IPoolableObject DisableObject()
    {
        gameObject.SetActive(false);
        return this;
    }

    public IPoolableObject EnableObject(Vector3 pos, Quaternion rot)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = pos;
        gameObject.transform.rotation = rot;

        return this;
    }

    public GameObject GameObject()
    {
        return gameObject;
    }

    #endregion
}
