using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance;
    public LayerMask ExplosionMask;
    public List<PrefabEnum> ExplosionGraphics;

    public float ExplosionGraphicDuration = 5f;

    private Collider[] results = new Collider[50];


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple ExplosionManagers");
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void SpawnExplosion(Vector3 position, float range, float power)
    {
        if(!ObjectPool.instance)
        {
            Debug.LogError("Explosion Request: No ObjectPool Found");
            return;
        }
        TriggerExplosion(position, range, power);
        HandleGraphics(position, range);
    }

    private void TriggerExplosion(Vector3 position, float range, float power)
    {
        int numHit = Physics.OverlapSphereNonAlloc(transform.position, range, results, ExplosionMask);
    }

    private void HandleGraphics(Vector3 position, float range)
    {
        Transform graphic = ObjectPool.instance.RequestObject(ExplosionGraphics[Random.Range(0, ExplosionGraphics.Count)], position, Quaternion.identity, true)
            .DelayedDisableObject(ExplosionGraphicDuration).GameObject().transform;

        graphic.localScale = Vector3.one * range;
    }
}