using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickPainter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Color color;
    [SerializeField] private float radius;
    [SerializeField] private float hardness;

    Collider[] results;

    RaycastHit hit;
    private void Update()
    {
        color.a = 1;
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                results = Physics.OverlapSphere(hit.point, radius);
                Debug.Log(results.Length+" - "+ radius+" - "+hit.point);
                for (int i = 0; i < results.Length; i++)
                {
                    PaintableObject paintableObject = results[i].GetComponent<PaintableObject>();
                    if (paintableObject != null)
                    {
                        PaintManager.instance.Paint(paintableObject, hit.point, radius, hardness, color);
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hit.point, radius);
    }
}
