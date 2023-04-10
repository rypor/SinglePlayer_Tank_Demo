using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineBrain brain;

    #region Built-in Methods

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple CameraManagers");
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        brain = GetComponent<CinemachineBrain>();
    }

    #endregion

    #region Public Custom Methods

    public void StartScreenShake(float duration, float intensity)
    {
        StopAllCoroutines();
        StartCoroutine(ScreenShake(brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>(), duration, intensity));
    }

    IEnumerator ScreenShake(CinemachineVirtualCamera cam, float duration, float intensity)
    {
        //float step = intensity / duration;
        CinemachineBasicMultiChannelPerlin channelPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        channelPerlin.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(duration);
        channelPerlin.m_AmplitudeGain = 0f;
    }

    #endregion
}
