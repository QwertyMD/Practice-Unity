using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform path;            // The path GameObject (a cube representing the footpath)
    public float speed = 2f;          // Speed of the NPC
    public float waypointSpacing = 2f; // Distance between generated waypoints

    private Vector3[] waypoints;      // Array of generated waypoints along the path
    private int currentWaypointIndex = 0; // Current waypoint the NPC is moving toward
    private Animator animator;        // Reference to the Animator component

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name + "! Please add an Animator component.");
        }

        // Set the walking animation parameter (if applicable)
        if (animator != null)
        {
            // Example: Set a bool parameter "IsWalking" to true
            // Adjust this based on your Animator setup
            animator.SetBool("IsWalking", true);
        }

        // Check if path is assigned
        if (path == null)
        {
            Debug.LogError("Path not assigned on " + gameObject.name + "! Please assign a path in the Inspector.");
            return;
        }

        // Generate waypoints along the path
        waypoints = GenerateWaypointsAlongPath(path);

        // Ensure there are waypoints to follow
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints generated for " + gameObject.name + "!");
            return;
        }

        // Start at the first waypoint
        transform.position = waypoints[0];
    }

    void Update()
    {
        // If no waypoints, do nothing
        if (waypoints == null || waypoints.Length == 0) return;

        // Move toward the current waypoint
        Vector3 targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

        // Look toward the target (only on X-Z plane)
        Vector3 direction = (targetWaypoint - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        // If close to the current waypoint, move to the next one
        if (Vector3.Distance(transform.position, targetWaypoint) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop back to the first waypoint
        }
    }

    Vector3[] GenerateWaypointsAlongPath(Transform pathTransform)
    {
        // Get the path's scale, position, and rotation
        Vector3 pathPosition = pathTransform.position;
        Vector3 pathScale = pathTransform.localScale;
        Quaternion pathRotation = pathTransform.rotation;

        // Determine the longest axis (X or Z) in local space
        float length;
        Vector3 localAxis;
        if (pathScale.x > pathScale.z)
        {
            length = pathScale.x;
            localAxis = Vector3.right; // X-axis in local space
        }
        else
        {
            length = pathScale.z;
            localAxis = Vector3.forward; // Z-axis in local space
        }

        // Calculate the number of waypoints
        int waypointCount = Mathf.Max(2, Mathf.CeilToInt(length / waypointSpacing));
        Vector3[] generatedWaypoints = new Vector3[waypointCount];

        // Generate waypoints along the path in local space, then transform to world space
        float startOffset = -length / 2f;
        for (int i = 0; i < waypointCount; i++)
        {
            float t = (float)i / (waypointCount - 1); // Normalized position (0 to 1)
            float offset = startOffset + (t * length);
            // Calculate the waypoint position in local space
            Vector3 localWaypointPosition = localAxis * offset;
            // Transform to world space using the path's rotation and position
            Vector3 worldWaypointPosition = pathPosition + (pathRotation * localWaypointPosition);
            // Ensure the waypoint is at the correct Y position (ground level)
            worldWaypointPosition.y = pathPosition.y;
            generatedWaypoints[i] = worldWaypointPosition;

            // Debug log to verify waypoint positions
            Debug.Log("Waypoint " + i + " for " + gameObject.name + ": " + generatedWaypoints[i]);
        }

        return generatedWaypoints;
    }

    // Visualize waypoints in the Scene view for debugging
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Gizmos.color = Color.blue;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i], 0.2f);
            if (i < waypoints.Length - 1)
            {
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }
        }
        // Draw a line from the last waypoint to the first to show the loop
        if (waypoints.Length > 1)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1], waypoints[0]);
        }
    }
}