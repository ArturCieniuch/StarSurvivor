using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool
{
    private ParticleController objectToSpawn;

    IObjectPool<ParticleController> pool;

    public ParticlePool(ParticleController objectToSpawn)
    {
        this.objectToSpawn = objectToSpawn;
        pool = new ObjectPool<ParticleController>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10);
    }

    public ParticleController GetFromPool()
    {
        return pool.Get();
    }

    public void ReturnToPool(ParticleController poolObject)
    {
        pool.Release(poolObject);
    }

    private ParticleController CreatePooledItem()
    {
        ParticleController poolObject = Object.Instantiate(objectToSpawn);

        return poolObject;
    }

    private void OnReturnedToPool(ParticleController poolObject)
    {
        poolObject.OnReturnToPool();
    }

    private void OnTakeFromPool(ParticleController poolObject)
    {
        poolObject.OnTakenFromPool();
    }

    private void OnDestroyPoolObject(ParticleController poolObject)
    {
        Object.Destroy(poolObject.gameObject);
    }
}
