using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioPlayer audioPrefab;

    private static GenericPool audioPool;

    private static Dictionary<string, int> activeAudioPlayersCount;

    private void Awake()
    {
        audioPool = new GenericPool(audioPrefab);
        activeAudioPlayersCount = new Dictionary<string, int>();
    }

    public static void PlaySound(SoundDataSO soundData, Vector3 position)
    {
        if (activeAudioPlayersCount.ContainsKey(soundData.name) && activeAudioPlayersCount[soundData.name] > 20)
        {
            return;
        }
        else
        {
            activeAudioPlayersCount.TryAdd(soundData.name, 0);
        }

        activeAudioPlayersCount[soundData.name]++;

        AudioPlayer player = audioPool.GetFromPool() as AudioPlayer;
        player.transform.position = position;
        player.Play(soundData);
    }

    public static void ReturnSound(AudioPlayer soundToReturn)
    {
        audioPool.ReturnToPool(soundToReturn);
        activeAudioPlayersCount[soundToReturn.name]--;
    }
}
