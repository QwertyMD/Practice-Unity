using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform path;
    public float speed = 2f;
    public float waypointSpacing = 2f;

    private Vector3[] waypoints;
    private int currentWaypointIndex = 0;
    private Animator animator;
    private bool isMoving = true;
    private float groundY;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (path == null)
        {
            return;
        }

        waypoints = GenerateWaypointsAlongPath(path);

        if (waypoints.Length == 0)
        {
            return;
        }

        groundY = waypoints[0].y;
        transform.position = new Vector3(waypoints[0].x, groundY, waypoints[0].z);
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0 || !isMoving) return;

        Vector3 targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
        newPosition.y = groundY;
        transform.position = newPosition;

        Vector3 direction = (targetWaypoint - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetWaypoint.x, 0, targetWaypoint.z)) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    Vector3[] GenerateWaypointsAlongPath(Transform pathTransform)
    {
        Vector3 pathPosition = pathTransform.position;
        Vector3 pathScale = pathTransform.localScale;
        Quaternion pathRotation = pathTransform.rotation;

        float length;
        Vector3 localAxis;
        if (pathScale.x > pathScale.z)
        {
            length = pathScale.x;
            localAxis = Vector3.right;
        }
        else
        {
            length = pathScale.z;
            localAxis = Vector3.forward;
        }

        int waypointCount = Mathf.Max(2, Mathf.CeilToInt(length / waypointSpacing));
        Vector3[] generatedWaypoints = new Vector3[waypointCount];

        float startOffset = -length / 2f;
        for (int i = 0; i < waypointCount; i++)
        {
            float t = (float)i / (waypointCount - 1);
            float offset = startOffset + (t * length);
            Vector3 localWaypointPosition = localAxis * offset;
            Vector3 worldWaypointPosition = pathPosition + (pathRotation * localWaypointPosition);
            worldWaypointPosition.y = pathPosition.y;
            generatedWaypoints[i] = worldWaypointPosition;
        }

        return generatedWaypoints;
    }

    public void SetMoving(bool moving)
    {
        isMoving = moving;
        if (animator != null)
        {
            animator.SetBool("IsWalking", moving);
        }

        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;
    }

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
        if (waypoints.Length > 1)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1], waypoints[0]);
        }
    }
}