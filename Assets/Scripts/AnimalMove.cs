using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float speed = 2f;
    public float rectangleWidth = 10f;
    public float rectangleHeight = 5f;

    private Vector3 centerPosition;
    private Vector3 targetPosition;

    void Start()
    {
        centerPosition = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        MoveWithinRectangle();
    }

    void MoveWithinRectangle()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        float randomX = centerPosition.x + Random.Range(-rectangleWidth / 2f, rectangleWidth / 2f);
        float randomZ = centerPosition.z + Random.Range(-rectangleHeight / 2f, rectangleHeight / 2f);
        targetPosition = new Vector3(randomX, centerPosition.y, randomZ);
    }
}