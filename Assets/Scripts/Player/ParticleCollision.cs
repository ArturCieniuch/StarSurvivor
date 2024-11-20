using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioPlayer audioPlayer;

    private float damage;

    public void Shot()
    {
        particleSystem.Play();
        audioPlayer.Play();
    }

    public void SetUp(float damage)
    {
        this.damage = damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = EnemyManager.Instance.GetEnemy(other.name);

            if (enemy == null)
            {
                return;
            }

            Debug.Log("DAMAGE " + damage);

            enemy.DealDamage(damage);
        }
    }
}
