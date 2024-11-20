using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public static PoolController Instance;

    private Dictionary<Type, Dictionary<string, GenericPool>> pools = new Dictionary<Type, Dictionary<string, GenericPool>>(5);

    private void Awake()
    {
        Instance = this;
    }

    public T GetObject<T>(PoolObject objectToGet) where T : PoolObject
    {
        Type objectType = objectToGet.GetType();

        if (!pools.ContainsKey(objectType))
        {
            pools.Add(objectType, new Dictionary<string, GenericPool>());
        }

        if (!pools[objectType].ContainsKey(objectToGet.objectName))
        {
            pools[objectType].Add(objectToGet.objectName, new GenericPool(objectToGet));
        }

        return pools[objectType][objectToGet.objectName].GetFromPool() as T;
    }

    public void ReturnObject(PoolObject objectToReturn)
    {
        pools[objectToReturn.GetType()][objectToReturn.objectName].ReturnToPool(objectToReturn);
    }
}

public enum ObjectType
{
    ENEMY,
    POINT,
    PARTICLE
}
