using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float rotationSpeed = 2f; 

    void Start()
    {
        targetPoint = 0;
    }

    void Update()
    {
        
        if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.1f)
        {
            increaseTargetInt();
        }

        
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

        
        Vector3 direction = (patrolPoints[targetPoint].position - transform.position).normalized;
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
}