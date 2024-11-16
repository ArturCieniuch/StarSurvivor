using System;
using System.Collections;
using UnityEngine;

public class Point : PoolObject
{
    public int pointValue;

    public float startSpeed;
    public float accelerationPerSecond;

    private Coroutine moveCoroutine;

    private void Update()
    {
        if (moveCoroutine == null && Vector3.Distance(transform.position, Player.Instance.transform.position) <= Player.Instance.pointRange)
        {
            StartCoroutine(MoveToPlayer());
        }
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
                Player.Instance.AddPoints(pointValue);
                EnemyManager.Instance.ReturnPoint(this);
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
}
