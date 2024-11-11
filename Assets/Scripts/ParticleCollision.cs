using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public ParticleSystem particleSystem;

    List<ParticleCollisionEvent> collsionEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemySpawner.Instance.RemoveEnemy(other.GetComponent<Enemy>());
        }
    }
}
