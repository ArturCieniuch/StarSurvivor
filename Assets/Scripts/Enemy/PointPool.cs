using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class PointPool 
{
    private Point pointToSpawn;

    IObjectPool<Point> pool;

    public PointPool(Point point)
    {
        this.pointToSpawn = point;
        pool = new ObjectPool<Point>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10);
    }

    public Point GetFromPool()
    {
        return pool.Get();
    }

    public void ReturnToPool(Point point)
    {
        pool.Release(point);
    }

    private Point CreatePooledItem()
    {
        Point point = Object.Instantiate(pointToSpawn);

        return point;
    }

    private void OnReturnedToPool(Point point)
    {
        point.OnReturnToPool();
    }

    private void OnTakeFromPool(Point point)
    {
        point.OnTakenFromPool();
    }

    private void OnDestroyPoolObject(Point point)
    {
        Object.Destroy(point.gameObject);
    }
}