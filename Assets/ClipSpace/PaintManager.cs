using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Networking.UnityWebRequest;

public class PaintManager : MonoBehaviour
{
    public static PaintManager instance;
    public Shader texturePaint;

    int hardnessID = Shader.PropertyToID("_Hardness");
    int textureID = Shader.PropertyToID("_MainTex");
    int colorID = Shader.PropertyToID("_PaintColor");
    int positionID = Shader.PropertyToID("_PaintPosition");
    int radiusID = Shader.PropertyToID("_Radius");

    Material paintMaterial;
    CommandBuffer command;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple Paint Managers");
            Destroy(this);
            return;
        }
        instance = this;

        paintMaterial = new Material(texturePaint);
        command = new CommandBuffer();
        command.name = "CommmandBuffer - " + gameObject.name;
    }

    public void InitTextures(PaintableObject paintableObject)
    {
        Paint(paintableObject, Vector3.zero, 999999, 1, new Color(0, 0, 0, 0));
    }

    private void Paint(PaintableObject paintableObject, Vector3 pos, float radius, float hardness, Color color)
    {
        RenderTexture mask = paintableObject.getMask();
        RenderTexture support = paintableObject.getSupport();
        Renderer rend = paintableObject.getRenderer();

        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetColor(colorID, color);
        paintMaterial.SetVector(positionID, pos);

        paintMaterial.SetTexture(textureID, mask);

        command.SetRenderTarget(support);
        command.DrawRenderer(rend, paintMaterial, 0);

        command.Blit(support, mask);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    Collider[] results;
    public void PaintSphere(Vector3 pos, float radius, float hardness, Color color)
    {
        results = Physics.OverlapSphere(pos, radius);
        //Debug.Log(results.Length + " - " + radius + " - " + pos);
        for (int i = 0; i < results.Length; i++)
        {
            PaintableObject paintableObject = results[i].GetComponent<PaintableObject>();
            if (paintableObject != null)
            {
                PaintManager.instance.Paint(paintableObject, pos, radius, hardness, color);
            }
        }
    }
}
