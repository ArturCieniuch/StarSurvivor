using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private bool trackX;
    [SerializeField] private bool trackY;
    [SerializeField] private bool trackZ;

    private Enemy enemyToTrack;

    public void SetTarget(Enemy enemyToTrack)
    {
        this.enemyToTrack = enemyToTrack;
    }

    void Update()
    {
        if (enemyToTrack == null)
        {
            transform.localRotation = Quaternion.identity;
            return;
        }

        Vector3 directionToEnemy = (enemyToTrack.transform.position - transform.position).normalized;
        Vector3 direction = Quaternion.LookRotation(directionToEnemy, Vector3.up).eulerAngles;
        Vector3 trackVector3 = Vector3.zero;

        if (trackX)
        {
            trackVector3.x = direction.x;
        }

        if (trackY)
        {
            trackVector3.y = direction.y;
        }

        if (trackZ)
        {
            trackVector3.z = direction.z;
        }

        transform.localRotation = Quaternion.Euler(trackVector3);
    }
}
