using System.Collections.Generic;
using UnityEngine;

public class ParticleController : PoolObject
{
    [SerializeField]
    private List<ParticleSystem> particles;

    public void Play()
    {
        foreach (var particle in particles)
        {
            particle.Play();
        }
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
        Stop();
    }

    public override void OnReturnToPool()
    {
        Stop();
    }
}
