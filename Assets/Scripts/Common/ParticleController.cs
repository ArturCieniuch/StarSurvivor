using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : PoolObject
{
    [SerializeField]
    private List<ParticleSystem> particles;

    public void Play(bool returnToPool)
    {
        float duration = 0;

        foreach (var particle in particles)
        {
            particle.Play();

            if(particle.main.duration > duration)
            {
                duration = particle.main.duration;
            }
        }

        if (returnToPool)
        {
            StartCoroutine(ReturnToPoolAfterTime(duration));
        }
    }

    private IEnumerator ReturnToPoolAfterTime(float time)
    {
        float timer = 0;

        while (timer < time)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        PoolController.Instance.ReturnObject(this);
    }

    public void Stop()
    {
        foreach (var particle in particles)
        {
            particle.Stop();
        }
    }

    public override void OnTakenFromPool()
    {

    }

    public override void OnReturnToPool()
    {
        Stop();
    }
}
