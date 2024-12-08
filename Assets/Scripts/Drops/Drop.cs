using System.Collections;
using UnityEngine;

public class Drop : PoolObject
{
    public DropType dropType;

    [SerializeField] private float startSpeed = 1;
    [SerializeField] private float accelerationPerSecond = 1.1f;

    private Coroutine moveCoroutine;

    private void Update()
    {
        if (CanBePickUp() && moveCoroutine == null && Vector3.Distance(transform.position, Player.Instance.transform.position) <= GetPickUpRange())
        {
            StartCoroutine(MoveToPlayer());
        }
    }

    protected virtual float GetPickUpRange()
    {
        return Player.Instance.pointRange;
    }

    protected virtual bool CanBePickUp()
    {
        return true;
    }

    private IEnumerator MoveToPlayer()
    {
        Transform playerTransform = Player.Instance.transform;

        float speed = startSpeed;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

            yield return null;

            speed += (speed * accelerationPerSecond * Time.deltaTime);

            if (Vector3.Distance(transform.position, Player.Instance.transform.position) < 1f)
            {
                OnCollect();
                EnemyManager.Instance.ReturnDrop(this);
                yield break;
            }
        }
    }

    public override void OnTakenFromPool()
    {
        gameObject.SetActive(true);
    }

    public override void OnReturnToPool()
    {
        gameObject.SetActive(false);
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = null;
    }

    protected virtual void OnCollect()
    {

    }
}
