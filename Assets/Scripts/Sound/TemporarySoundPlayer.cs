using System;
using UnityEngine;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
public class TemporarySoundPlayer : MonoBehaviour
{
    public event Action OnFinish;

    private AudioSource audioSource;
    public string ClipName {
        get
        {
            return audioSource.clip.name;
        }
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying && OnFinish != null)
        {
            OnFinish.Invoke();
            OnFinish = null;
        }
    }

    public void Play(AudioMixerGroup audioMixer, float delay, bool isLoop)
    {
        audioSource.outputAudioMixerGroup = audioMixer;
        audioSource.loop = isLoop;
        audioSource.Play();
    }

    public void InitSound2D(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void InitSound3D(AudioClip clip, float minDistance, float maxDistance)
    {
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
    }

    public void SetOnFinish(Action callback)
    {
        OnFinish = callback;
    }
}
