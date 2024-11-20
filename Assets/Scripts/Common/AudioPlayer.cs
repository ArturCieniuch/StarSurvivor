using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float basePitch;
    [SerializeField] private float pitchOffset;

    [SerializeField] private List<AudioClip> clips;

    public void Play()
    {
        audioSource.pitch = basePitch + Random.Range(-pitchOffset, pitchOffset);

        audioSource.clip = clips[Random.Range(0, clips.Count)];

        audioSource.Play();
    }
}
