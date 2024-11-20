using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField]
    private List<ParticleCollision> particles;

    [SerializeField] private bool volleyFire;

    private int index;

    public void SetUp(float damagePerShot)
    {
        float damage = damagePerShot;

        if (volleyFire)
        {
            damage = damagePerShot / particles.Count;
        }

        foreach (var particle in particles)
        {
            particle.SetUp(damage);
        }
    }

    public void Shot()
    {
        if (volleyFire)
        {
            foreach (var particle in particles)
            {
                particle.Shot();
            }

            return;
        }

        if (index >= particles.Count)
        {
            index = 0;
        }

        particles[index++].Shot();
    }
}
