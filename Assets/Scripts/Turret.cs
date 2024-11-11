using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Turret : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>(50);
    public float rotationSpeed = 5;

    public ParticleSystem particleSystem;

    public float fireRate;
    public float range;
    public bool shotOnTargetOnly;

    private float timer;

    private void Start()
    {
        EnemySpawner.Instance.enemyRemoved.AddListener(OnEnemyRemoved);
    }

    private void OnEnemyRemoved(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        enemies.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        enemies.Remove(enemy);
    }

    private Enemy FindClosestEnemy()
    {
        if (enemies.Count == 0)
        {
            return null;
        }

        Enemy closestEnemy = enemies[0];

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

            if (distanceToEnemy < Vector3.Distance(closestEnemy.transform.position, transform.position))
            {
                closestEnemy = enemy;
            }
        }


        if (Vector3.Distance(closestEnemy.transform.position, transform.position) > range)
        {
            closestEnemy = null;
        }

        return closestEnemy;
    }

    void Update()
    {
        Enemy closestEnemy = FindClosestEnemy();

        timer += Time.deltaTime;

        if (closestEnemy == null)
        {
            return;
        }

        Vector3 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
        directionToEnemy.y = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionToEnemy, Vector3.up), rotationSpeed * Time.deltaTime);

        if (timer >= fireRate)
        {
            if (!shotOnTargetOnly)
            {
                timer = 0;
                particleSystem.Play();
            } 
            else if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(directionToEnemy, Vector3.up)) == 0)
            {
                timer = 0;
                particleSystem.Play();
            }
        }
    }
}
