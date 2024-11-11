using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool
{
    private Enemy enemyToSpawn;
    private int maxPoolSize = 10;

    IObjectPool<Enemy> pool;

    public EnemyPool(Enemy enemy, int maxPoolSize)
    {
        this.enemyToSpawn = enemy;
        this.maxPoolSize = maxPoolSize;
        pool = new ObjectPool<Enemy>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10);
    }

    public Enemy GetFromPool()
    {
        return pool.Get();
    }

    public void ReturnToPool(Enemy enemy)
    {
        pool.Release(enemy);
    }

    private Enemy CreatePooledItem()
    {
        Enemy enemy = Object.Instantiate(enemyToSpawn);

        return enemy;
    }

    private void OnReturnedToPool(Enemy enemy)
    {
        enemy.OnReturnToPool();
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.OnTakenFromPool();
    }

    private void OnDestroyPoolObject(Enemy enemy)
    {
        Object.Destroy(enemy.gameObject);
    }
}