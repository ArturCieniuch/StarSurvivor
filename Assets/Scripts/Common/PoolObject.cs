using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    public string objectName;
    public abstract void OnTakenFromPool();
    public abstract void OnReturnToPool();
}