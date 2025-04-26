using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;          // Speed of the fish
    public float rectangleWidth = 10f; // Width of the rectangle (X-axis)
    public float rectangleHeight = 5f; // Height of the rectangle (Z-axis)
    public float waterLevel = 0f;     // Y position of the water
    public float jumpHeight = 2f;     // How high the fish jumps
    public float jumpDuration = 1f;   // How long the jump takes
    public AudioClip jumpSound;       // The sound to play when the fish jumps
    private const float SOUND_DISTANCE_THRESHOLD = 30f; // Fixed distance for sound to play

    private float jumpIntervalMin = 3f; // Minimum time between jumps (seconds)
    private float jumpIntervalMax = 7f; // Maximum time between jumps (seconds)
    private float jumpInterval;         // The randomized interval for this fish

    private Vector3 centerPosition;   // Center of the rectangle
    private Vector3 targetPosition;   // Where the fish is swimming to
    private float timeSinceLastJump = 0f; // Track time for jumping
    private float jumpTimer = 0f;     // Track progress of the jump
    private bool isJumping = false;   // Is the fish currently jumping?
    private AudioSource audioSource;  // To play the jump sound
    private GameObject player;        // Reference to the player

    void Start()
    {
        // Use the fish's initial position as the center of the rectangle
        centerPosition = transform.position;
        centerPosition.y = waterLevel;
        transform.position = centerPosition;

        // Randomize the jump interval for this fish
        jumpInterval = Random.Range(jumpIntervalMin, jumpIntervalMax);

        // Pick the first random target to swim to
        PickNewTarget();

        // Add an AudioSource component to this fish if it doesn't have one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = jumpSound;
        audioSource.playOnAwake = false;

        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player object has the 'Player' tag.");
        }
    }

    void Update()
    {
        // Update timers
        timeSinceLastJump += Time.deltaTime;

        // Check distance to the player and stop audio if too far
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > SOUND_DISTANCE_THRESHOLD && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Handle random movement within the rectangle
        MoveWithinRectangle();

        // Handle jumping
        HandleJump();
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
        targetPosition = new Vector3(randomX, waterLevel, randomZ);
    }

    void HandleJump()
    {
        // Check if it's time to jump
        if (timeSinceLastJump >= jumpInterval && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
            timeSinceLastJump = 0f;

            // Play the jump sound if the player is close enough
            if (audioSource != null && jumpSound != null && player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= SOUND_DISTANCE_THRESHOLD)
                {
                    audioSource.Play();
                }
            }
        }

        // If jumping, update the Y position
        if (isJumping)
        {
            jumpTimer += Time.deltaTime / jumpDuration;
            float height = Mathf.Sin(jumpTimer * Mathf.PI) * jumpHeight;
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(currentPos.x, waterLevel + height, currentPos.z);

            // End the jump when the timer reaches 1
            if (jumpTimer >= 1f)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x, waterLevel, transform.position.z);
            }
        }
    }
}