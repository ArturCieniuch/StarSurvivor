using System;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class GenericPool
{
    private PoolObject objectToSpawn;

    IObjectPool<PoolObject> pool;

    public GenericPool(PoolObject objectToSpawn)
    {
        this.objectToSpawn = objectToSpawn;
        pool = new ObjectPool<PoolObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10);
    }

    public PoolObject GetFromPool()
    {
        return pool.Get();
    }

    public void ReturnToPool(PoolObject poolObject)
    {
        pool.Release(poolObject);
    }

    private PoolObject CreatePooledItem()
    {
        PoolObject poolObject = Object.Instantiate(objectToSpawn);

        poolObject.name = Guid.NewGuid().ToString();

        return poolObject;
    }

    private void OnReturnedToPool(PoolObject poolObject)
    {
        poolObject.OnReturnToPool();
    }

    private void OnTakeFromPool(PoolObject poolObject)
    {
        poolObject.OnTakenFromPool();
    }

    private void OnDestroyPoolObject(PoolObject poolObject)
    {
        Object.Destroy(poolObject.gameObject);
    }
}

