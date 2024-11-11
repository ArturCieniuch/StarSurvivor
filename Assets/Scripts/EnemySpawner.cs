using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Spawn Info")]
    public Enemy enemyToSpawn;
    public int timeBetweenSpawns;
    public int enemiesInSpawnWave;
    public int maxEnemies;
    [Header("Distance")]
    public int minDistanceToPlayer;
    public int maxDistanceToPlayer;

    public List<Enemy> enemies;

    private float timer;
    private EnemyPool enemyPool;

    public UnityEvent<Enemy> enemyRemoved;

    private void Awake()
    {
        Instance = this;
        enemies = new List<Enemy>(maxEnemies);
        enemyPool = new EnemyPool(enemyToSpawn, maxEnemies);
        timer = timeBetweenSpawns;
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer >= timeBetweenSpawns)
        {
            for (int i = 0; i < enemiesInSpawnWave; i++)
            {
                SpawnEnemy();
            }

            timer = 0;
        }

        timer += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        if (enemies.Count >= maxEnemies)
        {
            return;
        }

        Vector2 point = Random.insideUnitCircle.normalized * Random.Range(minDistanceToPlayer, maxDistanceToPlayer);
        Vector3 enemyPosition = new Vector3(point.x, 0, point.y);

        Enemy enemy = enemyPool.GetFromPool();
        enemy.transform.position = enemyPosition;
        enemy.transform.rotation = Quaternion.identity;

        enemy.ResetTrail();

        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyRemoved.Invoke(enemyToRemove);
        enemies.Remove(enemyToRemove);
        enemyPool.ReturnToPool(enemyToRemove);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistanceToPlayer);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistanceToPlayer);
    }
}
