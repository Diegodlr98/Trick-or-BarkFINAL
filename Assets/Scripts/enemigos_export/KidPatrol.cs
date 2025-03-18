using UnityEngine;
using System.Collections;

public class KidPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int targetPoint;
    public float speed;
    public float rotationSpeed = 2f;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        targetPoint = 0;
    }

    void Update()
    {
        if (!isChasing)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.1f)
        {
            increaseTargetInt();
        }

        MoveTowards(patrolPoints[targetPoint].position);
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            MoveTowards(player.position);
        }
    }

    void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void increaseTargetInt()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isChasing = true;
            StopAllCoroutines();
            StartCoroutine(ChaseForSeconds(2f));
        }
    }

    IEnumerator ChaseForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isChasing = false;
    }
}
