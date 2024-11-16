using System;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class EnemyPool
{
    private Enemy enemyToSpawn;

    IObjectPool<Enemy> pool;

    public EnemyPool(Enemy enemy)
    {
        this.enemyToSpawn = enemy;
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

        enemy.name = Guid.NewGuid().ToString();

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