using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Delegates;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Spawn Info")]
    public Enemy enemyToSpawn;
    public Enemy enemyToSpawnFast;
    public Enemy enemyToSpawnBig;

    public Point pointPrefab;
    public int timeBetweenSpawns;
    public int enemiesInSpawnWave;
    public int maxEnemies;
    [Header("Difficulty")] 
    public float timeBetweenChange;
    public int difficultyChange;

    [Header("Drops")] public List<Drop> drops;
    private Dictionary<DropType, Drop> dropsDictionary;

    [Header("Distance")]
    public int minDistanceToPlayer;
    public int maxDistanceToPlayer;
    [SerializeField] private bool showDebug;

    public Dictionary<string, Enemy> enemies;

    private float timer;
    private float difficultyTimer;

    public OnEnemy enemyRemoved;

    private void Awake()
    {
        Instance = this;
        enemies = new Dictionary<string, Enemy>(maxEnemies);
        timer = timeBetweenSpawns;

        dropsDictionary = new Dictionary<DropType, Drop>(drops.Count);

        foreach (var drop in drops)
        {
            dropsDictionary.Add(drop.dropType, drop);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer >= timeBetweenSpawns)
        {
            for (int i = 0; i < enemiesInSpawnWave; i++)
            {
                if (i < 8)
                {
                    SpawnEnemy(enemyToSpawn);
                } 
                else
                {
                    double value = GameController.Rand.NextDouble();

                    if (value < 0.5f)
                    {
                        SpawnEnemy(enemyToSpawn);
                    }
                    else if (value < 0.75f)
                    {
                        SpawnEnemy(enemyToSpawnBig);
                    }
                    else
                    {
                        SpawnEnemy(enemyToSpawnFast);
                    }
                }
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

    private void SpawnEnemy(Enemy enemyPrefab)
    {
        if (enemies.Count >= maxEnemies)
        {
            return;
        }

        Vector3 playerPosition = Player.Instance.transform.position;

        Vector2 point = Random.insideUnitCircle.normalized * GameController.Rand.Next(minDistanceToPlayer, maxDistanceToPlayer);
        Vector3 enemyPosition = new Vector3(point.x + playerPosition.x, 0, point.y + playerPosition.z + transform.position.z);

        Enemy enemy = PoolController.Instance.GetObject<Enemy>(enemyPrefab);
        enemy.transform.position = enemyPosition;

        enemy.StartMovement();

        enemies.Add(enemy.name, enemy);
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyRemoved?.Invoke(enemyToRemove);
        enemies.Remove(enemyToRemove.name);
        PoolController.Instance.ReturnObject(enemyToRemove);
    }

    public void EnemyDrop(Vector3 position, DropData dropData)
    {
        if (dropData.dropType == DropType.EMPTY)
        {
            return;
        }

        Drop drop = dropsDictionary[dropData.dropType];
        Drop newDrop = PoolController.Instance.GetObject<Drop>(drop);
        newDrop.transform.position = position;
    }

    public void ReturnDrop(Drop drop)
    {
        PoolController.Instance.ReturnObject(drop);
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
