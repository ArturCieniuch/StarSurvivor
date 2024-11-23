using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField]
    private List<ParticleCollision> particles;

    [SerializeField] private bool volleyFire;
    [SerializeField] private SoundDataSO soundData;

    private int index;

    private Turret parentTurret;

    public void SetUp(Turret parentTurret)
    {
        this.parentTurret = parentTurret;

        foreach (var particle in particles)
        {
            particle.OnHit += OnHit;
        }
    }

    private float GetDamage()
    {
        float damage = parentTurret.GetDamage();

        if (volleyFire)
        {
            damage /= particles.Count;
        }

        return damage;
    }

    public void Shot()
    {
        if (volleyFire)
        {
            foreach (var particle in particles)
            {
                particle.Shot();
                AudioManager.PlaySound(soundData, transform.position);
            }

            return;
        }

        if (index >= particles.Count)
        {
            index = 0;
        }

        particles[index++].Shot();
        AudioManager.PlaySound(soundData, transform.position);
    }

    private void OnHit(GameObject other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        Enemy enemy = EnemyManager.Instance.GetEnemy(other.name);

        if (enemy == null)
        {
            return;
        }

        enemy.DealDamage(GetDamage());
    }
}
