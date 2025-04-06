using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [Header("Settings")]
    public Transform player; // Drag your character here
    public float openDistance = 3f; // Distance to trigger opening
    public float openAngle = 90f; // How much the door opens (in degrees)
    public float openSpeed = 2f; // How fast it opens/closes

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < openDistance && !isOpen)
        {
            // Open door smoothly
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openRotation,
                openSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, openRotation) < 1f)
            {
                isOpen = true;
            }
        }
        else if (distance >= openDistance && isOpen)
        {
            // Close door smoothly
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                closedRotation,
                openSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, closedRotation) < 1f)
            {
                isOpen = false;
            }
        }
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, openDistance);
    }
}