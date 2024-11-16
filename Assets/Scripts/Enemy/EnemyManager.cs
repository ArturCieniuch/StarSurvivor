using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Spawn Info")]
    public Enemy enemyToSpawn;
    public Point pointPrefab;
    public int timeBetweenSpawns;
    public int enemiesInSpawnWave;
    public int maxEnemies;
    [Header("Distance")]
    public int minDistanceToPlayer;
    public int maxDistanceToPlayer;
    [SerializeField] private bool showDebug;

    public Dictionary<string, Enemy> enemies;

    private float timer;

    public UnityEvent<Enemy> enemyRemoved;

    private void Awake()
    {
        Instance = this;
        enemies = new Dictionary<string, Enemy>(maxEnemies);
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
        Vector3 enemyPosition = new Vector3(point.x + transform.position.x, 0, point.y + transform.position.z);
        Enemy enemy = enemyPool.GetFromPool();
        enemy.transform.position = enemyPosition;
        Vector3 direction = (Player.Instance.transform.position - enemyPosition).normalized;
        enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        enemy.ResetTrail();

        enemies.Add(enemy.name, enemy);
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyRemoved.Invoke(enemyToRemove);
        enemies.Remove(enemyToRemove.name);
        enemyPool.ReturnToPool(enemyToRemove);
    }

    public void SpawnPoint(Vector3 position)
    {
        Point point = pointPool.GetFromPool();
        point.transform.position = position;
    }

    public void ReturnPoint(Point pointToReturn)
    {
        pointPool.ReturnToPool(pointToReturn);
    }

    public Enemy GetEnemy(string name)
    {
        return enemies[name];
    }

    private void OnDrawGizmosSelected()
    {
                if (showDebug)
        {
            Debug.DrawArc(0, 360, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up), minDistanceToPlayer, Color.green, false, false);
            Debug.DrawArc(0, 360, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up), maxDistanceToPlayer, Color.red, false, false);
        }

    }
}
