using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Scriptable Objects/SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public bool loop;
    [Range(0, 1)]
    public float volume;
    public float basePitch = 1;
    public float pitchOffset;
    public List<AudioClip> clips;
}
