using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Properties

    public static AudioManager instance;
    [SerializeField] private List<AudioType> _audioData;

    private Dictionary<AudioTypeEnum, AudioType> _audioTypes;

    #endregion

    #region Built-in Methods

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple AudioManagers");
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        SetupDict();
    }

    #endregion

    #region Public Custom Methods

    public Transform PlaySoundAtPoint(AudioTypeEnum type, Vector3 pos)
    {
        AudioData audio = _audioTypes[type].GetRandomAudioClip();

        AudioSource source = ObjectPool.instance.RequestObject(PrefabEnum.AudioSource, pos, Quaternion.identity, true).DelayedDisableObject(audio.clip.length+1).GameObject().GetComponent<AudioSource>();

        source.clip = audio.clip;
        source.volume = audio.volume;
        source.Play();
        return source.transform;
    }

    #endregion

    #region Private Custom Methods

    private void SetupDict()
    {
        _audioTypes = new Dictionary<AudioTypeEnum, AudioType>();
        foreach (AudioType audio in _audioData)
        {
            if (_audioTypes.ContainsKey(audio.Type))
                continue;
            _audioTypes.Add(audio.Type, audio);
        }
    }

    #endregion
}

public enum AudioTypeEnum
{
    StandardTankFire,
    StandardExplosion,
}