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
        
        objectPoolDict = new Dictionary<PrefabEnum, List<IPoolableObject>>();
        foreach (var prefab in prefabLinkDict.Keys)
        {
            InitPrefabPool(prefab);
        }
    }

    #endregion


    #region Public Custom Methods

    // FORCE CREATION ON POOL EMPTY NOT IMPLEMENTED
    public IPoolableObject RequestObject(PrefabEnum prefabEnum, Vector3 pos, Quaternion rot, bool forceCreationOnPoolEmpty)
    {
        List<IPoolableObject> pool = objectPoolDict[prefabEnum];

        if (pool.Count > 0)
        {
            IPoolableObject po = pool[0];
            pool.RemoveAt(0);

            po.EnableObject(pos, rot);
            return po;
        }

        //  If the object pool is empty
        return CreatePoolableObjectDisabled(prefabEnum).EnableObject(pos, rot);
    }

    public void ReturnObject(IPoolableObject poolableObject)
    {
        Debug.Log("Returning Object: " + poolableObject.GameObject());
        if(poolableObject.Active)
            poolableObject.DisableObject();
        objectPoolDict[poolableObject.PrefabEnumValue].Add(poolableObject);
    }

    #endregion

    #region Private Custom Methods

    private void InitPrefabPool(PrefabEnum prefabEnum)
    {
        List<IPoolableObject> pool = new List<IPoolableObject>();
        objectPoolDict.Add(prefabEnum, pool);

        for (int i=0; i<numInitPrefabs; i++)
        {
            CreatePoolableObjectDisabled(prefabEnum);
        }
    }

    // Creates Disabled instance of prefabEnum
    private IPoolableObject CreatePoolableObjectDisabled(PrefabEnum prefabEnum)
    {
        Debug.Log(prefabEnum);
        GameObject g = prefabLinkDict[prefabEnum];
        IPoolableObject poolableObject = Instantiate(g, Vector3.zero, Quaternion.identity).GetComponent<IPoolableObject>();
        if(poolableObject.Equals(null))
        {
            Debug.LogError(prefabEnum + " Prefab does not contain IPoolableObject Script!");
            return null;
        }
        poolableObject.PrefabEnumValue = prefabEnum;
        poolableObject.DisableObject();

        return poolableObject;
    }

    #endregion


    #region Debug

    #endregion
}

public enum PrefabEnum
{
    StandardReticle,
    StandardTankBullet,
    ExplosionGraphic_1,
    AudioSource,
}
