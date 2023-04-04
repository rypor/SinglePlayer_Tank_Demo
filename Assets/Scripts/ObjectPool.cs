using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Properties

    public static ObjectPool instance;


    [SerializeField] private int numInitPrefabs = 10;

    [SerializeField] private PrefabObjectDictionary prefabLinkDict;
    private Dictionary<PrefabEnum, List<IPoolableObject>> objectPoolDict;


    #endregion


    #region Built-in Methods

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple Object Pools!");
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        foreach(var prefab in prefabLinkDict.Keys)
        {
            InitPrefabPool(prefab);
        }
    }

    #endregion


    #region Public Custom Methods

    // FORCE CREATION ON POOL EMPTY NOT IMPLEMENTED
    public IPoolableObject RequestObject(PrefabEnum prefabEnum, Vector3 pos, Quaternion rot, bool forceCreationOnPoolEmpty)
    {
        List<IPoolableObject> pool = new List<IPoolableObject>();

        if (pool.Count > 0)
        {
            IPoolableObject po = pool[0];
            pool.RemoveAt(0);

            po.EnableObject(pos, rot);
            return po;
        }

        //  If the object pool is empty
        return CreatePoolableObject(prefabEnum).EnableObject(pos, rot);
    }

    public void ReturnObject(IPoolableObject poolableObject)
    {
        poolableObject.DisableObject();
        objectPoolDict[poolableObject.PrefabEnumValue].Add(poolableObject);
    }

    #endregion

    #region Private Custom Methods

    private void InitPrefabPool(PrefabEnum prefabEnum)
    {
        List<IPoolableObject> list = new List<IPoolableObject>();

        for(int i=0; i<numInitPrefabs; i++)
        {
            IPoolableObject poolableObject = CreatePoolableObject(prefabEnum);
            list.Add(poolableObject);
        }
    }

    // Creates Disabled instance of prefabEnum
    private IPoolableObject CreatePoolableObject(PrefabEnum prefabEnum)
    {
        GameObject g = prefabLinkDict[prefabEnum];
        IPoolableObject poolableObject = Instantiate(g, Vector3.zero, Quaternion.identity).GetComponent<IPoolableObject>();
        poolableObject.DisableObject();
        poolableObject.PrefabEnumValue = prefabEnum;

        return poolableObject;
    }

    #endregion


    #region Debug

    #endregion
}

public enum PrefabEnum
{
    StandardTankBullet,
}
