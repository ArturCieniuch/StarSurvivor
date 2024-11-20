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
    [Header("Difficulty")] 
    public float timeBetweenChange;
    public int difficultyChange;

    [Header("Distance")]
    public int minDistanceToPlayer;
    public int maxDistanceToPlayer;
    [SerializeField] private bool showDebug;

    public Dictionary<string, Enemy> enemies;

    private float timer;
    private float difficultyTimer;

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

        if (difficultyTimer >= timeBetweenChange)
        {
            enemiesInSpawnWave += difficultyChange;
            difficultyTimer = 0;
        }

        timer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

    }

    private void SpawnEnemy()
    {
        if (enemies.Count >= maxEnemies)
        {
            return;
        }

        Vector3 playerPosition = Player.Instance.transform.position;

        Vector2 point = Random.insideUnitCircle.normalized * Random.Range(minDistanceToPlayer, maxDistanceToPlayer);
        Vector3 enemyPosition = new Vector3(point.x + playerPosition.x, 0, point.y + playerPosition.z + transform.position.z);

        Enemy enemy = PoolController.Instance.GetObject<Enemy>(enemyToSpawn);
        enemy.transform.position = enemyPosition;
        //Vector3 direction = (enemyPosition - playerPosition).normalized;
        //enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        enemy.StartMovement();

        enemies.Add(enemy.name, enemy);
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyRemoved.Invoke(enemyToRemove);
        enemies.Remove(enemyToRemove.name);
        PoolController.Instance.ReturnObject(enemyToRemove);
    }

    public void SpawnPoint(Vector3 position)
    {
        Point point = PoolController.Instance.GetObject<Point>(pointPrefab);
        point.transform.position = position;
    }

    public void ReturnPoint(Point pointToReturn)
    {
        PoolController.Instance.ReturnObject(pointToReturn);
    }

    public Enemy GetEnemy(string name)
    {
        return enemies.ContainsKey(name) ? enemies[name] : null;
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
