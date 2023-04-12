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

    public void SpawnExplosion(Vector3 position, float range, float power, float upwardsModifier)
    {
        if(!ObjectPool.instance)
        {
            Debug.LogError("Explosion Request: No ObjectPool Found");
            return;
        }
        TriggerExplosion(position, range, power, upwardsModifier);
        HandleGraphics(position, range);
    }

    // Handles Explosion Physics
    private void TriggerExplosion(Vector3 position, float range, float power, float upwardsModifier)
    {
        int numHit = Physics.OverlapSphereNonAlloc(position, range, results, ExplosionMask);
        Debug.Log("nH: " + numHit);
        for (int i = 0; i < numHit; i++)
        {
            Rigidbody rb = results[i].attachedRigidbody;

            if (rb == null)
                continue;

            rb.AddExplosionForce(power, position, range, upwardsModifier, ForceMode.Impulse);

            ITankController controller = rb.GetComponent<ITankController>();
            //if (controller != null) controller.StopMotion();
        }
    }

    // Handles Explosion Graphics
    private void HandleGraphics(Vector3 position, float range)
    {
        Transform graphic = ObjectPool.instance.RequestObject(ExplosionGraphics[Random.Range(0, ExplosionGraphics.Count)], position, Quaternion.identity, true)
            .DelayedDisableObject(ExplosionGraphicDuration).GameObject().transform;

        graphic.localScale = Vector3.one * range;
    }
}