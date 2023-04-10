using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Type", menuName = "General/Audio Type")]
public class AudioType : ScriptableObject
{
    public AudioTypeEnum Type;
    public List<AudioData> AudioClips;

    public AudioData GetRandomAudioClip()
    {
        return AudioClips[UnityEngine.Random.Range(0, AudioClips.Count)];
    }
}

[Serializable]
public struct AudioData
{
    public AudioClip clip;
    public float volume;
}