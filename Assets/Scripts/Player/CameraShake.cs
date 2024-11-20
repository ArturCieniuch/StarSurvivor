using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeTime = 0.25f;
    public CinemachineBasicMultiChannelPerlin noisePerlin;
    public float amplitudeGain = 2;
    public float frequencyGain = 2;
    private Coroutine cameraShakeCoroutine;

    public void ShakeCamera()
    {
        if (cameraShakeCoroutine != null)
        {
            StopCoroutine(cameraShakeCoroutine);
        }
        cameraShakeCoroutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float timer = 0;

        while (timer < shakeTime)
        {
            noisePerlin.AmplitudeGain = amplitudeGain;
            noisePerlin.FrequencyGain = frequencyGain;

            timer += Time.deltaTime;
            yield return null;
        }

        noisePerlin.AmplitudeGain = 0;
        noisePerlin.FrequencyGain = 0;
    }
}
