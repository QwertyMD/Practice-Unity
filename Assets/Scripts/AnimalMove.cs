using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float speed = 2f;          // Speed of the horse
    public float rectangleWidth = 10f; // Width of the rectangle (X-axis)
    public float rectangleHeight = 5f; // Height of the rectangle (Z-axis)

    private Vector3 centerPosition;   // Center of the rectangle
    private Vector3 targetPosition;   // Where the horse is moving to

    void Start()
    {
        // Use the horse's initial position as the center of the rectangle
        centerPosition = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        MoveWithinRectangle();
    }

    void MoveWithinRectangle()
    {
        // Move toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Look toward the target (only on X-Z plane)
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        // If close to the target, pick a new random target
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