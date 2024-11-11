using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Rigidbody enemyRigidbody;

    private Coroutine movementCoroutine;

    public int damage;
    public TrailRenderer trail;

    [Header("Movement")]
    public float speed;
    public float waitTime;
    public float refreshTargetTime;
    [Range(0, 180)]
    public int rotationDegrees;
    [Range(0f, 5f)]
    public float trackingSpeed;

    private void Start()
    {
        StartMovement();
    }

    public void StartMovement()
    {
        movementCoroutine = StartCoroutine(Movement());
    }

    private Vector3 GetPlayerPosition()
    {
        return Player.Instance.transform.position;
    }

    private Vector3 GetPlayerDirection()
    {
        return (GetPlayerPosition() - transform.position).normalized;
    }

    private IEnumerator Movement()
    {
        float time = refreshTargetTime;
        float timer = time;

        float waitTimer = waitTime;

        Vector3 oldDirection = Vector3.zero;
        Vector3 moveDirection = Vector3.zero;
        float trackingProgress = 0;

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (true)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= time)
            {
                if (waitTimer < waitTime)
                {
                    enemyRigidbody.linearVelocity = Vector2.zero;

                    waitTimer += Time.fixedDeltaTime;

                    yield return wait;
                    continue;
                }

                oldDirection = enemyRigidbody.linearVelocity.normalized * speed;
                moveDirection = Quaternion.AngleAxis(Random.Range(-rotationDegrees, rotationDegrees), Vector3.forward) * GetPlayerDirection() * speed;

                timer = 0;
                waitTimer = 0;
                trackingProgress = 0;
            }

            if (trackingSpeed > 0)
            {
                trackingProgress += trackingSpeed * Time.fixedDeltaTime;

                trackingProgress = Mathf.Clamp01(trackingProgress);

                enemyRigidbody.linearVelocity = (Vector3.Lerp(oldDirection, moveDirection, trackingProgress));
            }
            else
            {
                enemyRigidbody.linearVelocity = moveDirection;
            }

            yield return wait;
        }
    }

    public void ResetTrail()
    {
        trail.Clear();
    }

    public void OnTakenFromPool()
    {
        gameObject.SetActive(true);
        trail.transform.SetParent(transform);
        StartMovement();
    }

    public void OnReturnToPool()
    {
        trail.transform.SetParent(null);
        enemyRigidbody.linearVelocity = Vector3.zero;
        enemyRigidbody.angularVelocity = Vector3.zero;
        StopCoroutine(movementCoroutine);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Player.Instance.DealDamage(damage);
            EnemySpawner.Instance.RemoveEnemy(this);
        }
    }

    public void Reset()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }
}
