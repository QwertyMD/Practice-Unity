using UnityEngine;

public class WhaleRectangleMovement : MonoBehaviour
{
    public float speed = 3f;          // Speed of the whale
    public float pauseTime = 1f;      // Pause time at each corner
    public float rectangleWidth = 15f; // Width of the rectangle (longer side, X-axis)
    public float rectangleHeight = 5f; // Height of the rectangle (shorter side, Z-axis)

    private Vector3[] waypoints;      // The four corners of the rectangle
    private int currentWaypointIndex = 0; // Track which waypoint we're heading to
    private Vector3 startPosition;    // Whale's starting position
    private bool isMoving = true;     // Track if the whale is moving

    void Start()
    {
        // Store the whale's starting position
        startPosition = transform.position;

        // Define the rectangle's corners relative to the starting position
        waypoints = new Vector3[4];
        waypoints[0] = startPosition;                          // Bottom-left (start)
        waypoints[1] = startPosition + new Vector3(rectangleWidth, 0, 0); // Bottom-right
        waypoints[2] = startPosition + new Vector3(rectangleWidth, 0, rectangleHeight); // Top-right
        waypoints[3] = startPosition + new Vector3(0, 0, rectangleHeight); // Top-left

        // Start the movement loop
        StartCoroutine(MoveInRectangle());
    }

    System.Collections.IEnumerator MoveInRectangle()
    {
        while (true) // Infinite loop
        {
            if (isMoving)
            {
                // Move toward the current waypoint
                Vector3 targetPosition = waypoints[currentWaypointIndex];
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Look toward the target (for better visuals)
                Vector3 direction = (targetPosition - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Rotate only on Y-axis
                }

                // Check if we've reached the waypoint
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isMoving = false;
                    currentWaypointIndex = (currentWaypointIndex + 1) % 4; // Move to next waypoint (loop back to 0)
                    yield return new WaitForSeconds(pauseTime); // Pause at the corner
                    isMoving = true;
                }
            }
            yield return null; // Wait for the next frame
        }
    }
}