using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePoolObject : MonoBehaviour, IPoolableObject
{

    #region Properties

    private PrefabEnum prefabEnumValue;
    private bool status;

    #endregion

    #region IPoolableObject Methods

    public PrefabEnum PrefabEnumValue { get => prefabEnumValue; set => prefabEnumValue = value; }
    public bool Active { get => status; }

    public IPoolableObject DisableObject()
    {
        gameObject.SetActive(false);
        status = false;
        ObjectPool.instance.ReturnObject(this);
        return this;
    }

    public IPoolableObject EnableObject(Vector3 pos, Quaternion rot)
    {
        gameObject.transform.position = pos;
        gameObject.transform.rotation = rot;
        gameObject.SetActive(true);
        status = true;

        return this;
    }

    public IPoolableObject DelayedDisableObject(float time)
    {
        StartCoroutine(Delay(time));
        return this;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        DisableObject();
    }

    public GameObject GameObject()
    {
        return gameObject;
    }

    #endregion
}
