using UnityEngine;

public class TicketInteraction : MonoBehaviour
{
    public Canvas ticketCanvas;        // The canvas with the "Click to buy ticket" text
    public GameObject doorPivot1;      // The pivot point of the first door
    public GameObject doorPivot2;      // The pivot point of the second door
    public AudioClip cashSound;        // The cash audio clip to play
    public float doorOpenAngle1 = 90f; // How much the first door rotates (degrees)
    public float doorOpenAngle2 = 90f; // How much the second door rotates (degrees)
    public float doorTransitionTime = 1f; // Duration of the door opening animation (seconds)

    private Camera playerCamera;       // The player's camera (for raycasting)
    private AudioSource audioSource;   // To play the cash sound
    private bool hasOpenedDoors = false; // Track if the doors are already opened

    void Start()
    {
        // Get the camera component (since the script is on the MainCamera)
        playerCamera = GetComponent<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("Camera component not found on this GameObject!");
        }

        // Add an AudioSource to this GameObject if it doesn't have one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = cashSound;
    }

    void Update()
    {
        // Check for left-click (Fire1 = left mouse button by default)
        if (Input.GetButtonDown("Fire1") && !hasOpenedDoors)
        {
            Debug.Log("Left-click detected!");

            // Cast a ray from the center of the camera (where the crosshair is)
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            // Check if the ray hits something (max distance 100 units)
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                // Check if the hit object is the canvas (or a child of the canvas)
                Canvas hitCanvas = hit.collider.GetComponentInParent<Canvas>();
                if (hitCanvas != null && hitCanvas == ticketCanvas)
                {
                    Debug.Log("Hit the ticket canvas!");

                    // Play the cash sound
                    audioSource.Play();

                    // Start the door opening animation
                    StartCoroutine(OpenDoors());

                    // Mark the doors as opened so they don't open again
                    hasOpenedDoors = true;

                    // Optional: Hide the canvas after buying the ticket
                    ticketCanvas.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything!");
            }
        }
    }

    // Coroutine to animate the door opening smoothly
    System.Collections.IEnumerator OpenDoors()
    {
        float elapsedTime = 0f;
        // Store the starting rotations of both doors
        Quaternion startRotation1 = doorPivot1.transform.rotation;
        Quaternion startRotation2 = doorPivot2.transform.rotation;
        // Calculate the target rotations
        Quaternion endRotation1 = startRotation1 * Quaternion.Euler(0, doorOpenAngle1, 0);
        Quaternion endRotation2 = startRotation2 * Quaternion.Euler(0, doorOpenAngle2, 0);

        // Animate over doorTransitionTime seconds
        while (elapsedTime < doorTransitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / doorTransitionTime; // Progress (0 to 1)
            // Smoothly interpolate between start and end rotations
            doorPivot1.transform.rotation = Quaternion.Lerp(startRotation1, endRotation1, t);
            doorPivot2.transform.rotation = Quaternion.Lerp(startRotation2, endRotation2, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the doors are at their final rotations
        doorPivot1.transform.rotation = endRotation1;
        doorPivot2.transform.rotation = endRotation2;
    }
}