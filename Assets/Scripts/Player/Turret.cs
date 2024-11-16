using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class Turret : MonoBehaviour
{
    private List<Enemy> enemies;

    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float fireRate;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private float range;
    [SerializeField] private bool shotOnTargetOnly;

    [SerializeField] private int minRightAngle;
    [SerializeField] private int maxRightAngle;
    [SerializeField] private int minLeftAngle;
    [SerializeField] private int maxLeftAngle;
    [SerializeField] private bool showDebug;

    [Header("Components")]
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private SphereCollider trigger;
    [SerializeField] private ParticleCollision particleCollision;
    [SerializeField] private EnemyTracker enemyTracker;
    [SerializeField] private Transform shipTransform;
    [SerializeField] private Transform angleTracker;


    private float timer;

    private void Start()
    {
        EnemyManager.Instance.enemyRemoved.AddListener(OnEnemyRemoved);
        enemies = new List<Enemy>(EnemyManager.Instance.maxEnemies);
        particleCollision.SetUp(damagePerSecond / fireRate);
        trigger.radius = range;
        angleTracker.SetParent(shipTransform);
    }

    private void OnEnemyRemoved(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        enemies.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        enemies.Remove(enemy);
    }

    private Enemy FindClosestEnemy()
    {
        if (enemies.Count == 0)
        {
            return null;
        }

        Enemy closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (!CheckAngleAndDistance(enemy.transform))
            {
                continue;
            }

            if (closestEnemy == null)
            {
                closestEnemy = enemy;
                continue;
            }

            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

            if (distanceToEnemy < Vector3.Distance(closestEnemy.transform.position, transform.position))
            {
                closestEnemy = enemy;
            }
        }

        if (closestEnemy == null || !CheckAngleAndDistance(closestEnemy.transform))
        {
            return null;
        }

        return closestEnemy;
    }

    private bool CheckAngleAndDistance(Transform enemyTransform)
    {
        if (Vector3.Distance(enemyTransform.position, transform.position) > range)
        {
            return false;
        }

        Vector3 directionToEnemy = (enemyTransform.position - transform.position).normalized;
        directionToEnemy.y = 0;

        Vector3 directionToTracker = (angleTracker.position - transform.position).normalized;

        float angle = Vector3.SignedAngle(directionToTracker, directionToEnemy, Vector3.up);

        if (angle > 0)
        {
            return angle >= minRightAngle && angle <= maxRightAngle;
        } 
        else
        {
            return angle <= -minLeftAngle && angle >= -maxLeftAngle;

        }
    }

    private Vector3 GetTrackerDirection()
    {
        return (angleTracker.position - transform.position).normalized;
    }

    void Update()
    {
        Enemy closestEnemy = FindClosestEnemy();

        timer += Time.deltaTime;

        enemyTracker.SetTarget(closestEnemy);

        if (closestEnemy == null)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GetTrackerDirection()), rotationSpeed * Time.deltaTime);
            return;
        }

        Vector3 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
        directionToEnemy.y = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionToEnemy, Vector3.up), rotationSpeed * Time.deltaTime);

        float realFireRate = 1f / fireRate;

        if (timer >= realFireRate)
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

    private void OnDrawGizmosSelected()
    {
        if (showDebug)
        {
            Debug.DrawArc(-maxRightAngle, -minRightAngle, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up), range, Color.blue, false, true);
            Debug.DrawArc(minLeftAngle, maxLeftAngle, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up), range, Color.blue, false, true);
        }
    }
}
