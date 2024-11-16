using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;

    private float damage;

    public void SetUp(float damage)
    {
        this.damage = damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager.Instance.GetEnemy(other.name).DealDamage(damage);
        }
    }
}
