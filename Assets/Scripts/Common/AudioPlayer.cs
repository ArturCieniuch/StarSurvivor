using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class AudioPlayer : PoolObject
{
    [SerializeField] private AudioSource audioSource;
    public void Play(SoundDataSO soundData)
    {
        gameObject.name = soundData.name;

        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.volume = soundData.volume;

        audioSource.pitch = soundData.basePitch + Random.Range(-soundData.pitchOffset, soundData.pitchOffset);

        audioSource.clip = soundData.clips[Random.Range(0, soundData.clips.Count)];

        audioSource.loop = soundData.loop;
        audioSource.Play();
    }

    private void Update()
    {
        if (audioSource.loop || audioSource.isPlaying)
        {
            return;
        }

        AudioManager.ReturnSound(this);
    }

    public override void OnTakenFromPool()
    {
        gameObject.SetActive(true);
    }

    public override void OnReturnToPool()
    {
        gameObject.SetActive(false);
        audioSource.Stop();
    }
}
