using UnityEngine;

public class StagAudio : MonoBehaviour
{
    public AudioClip stagSound;       // The sound to play for the stag
    private const float SOUND_DISTANCE_THRESHOLD = 30f; // Fixed distance for sound to play

    private AudioSource audioSource;  // To play the stag sound
    private GameObject player;        // Reference to the player
    private Animator animator;        // Reference to the stag's Animator
    private float lastPlayTime = -10f; // Track when the sound was last played (negative to play immediately)
    private float animationLength;    // Length of the animation clip

    void Start()
    {
        // Add an AudioSource component to this stag if it doesn't have one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = stagSound;
        audioSource.playOnAwake = false;

        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player object has the 'Player' tag.");
        }

        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the stag!");
        }
        else
        {
            // Get the length of the current animation clip
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            animationLength = stateInfo.length / animator.speed; // Adjusted for Animator speed
        }
    }

    void Update()
    {
        // Check distance to the player
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // If the player is too far, stop the audio if it's playing
            if (distanceToPlayer > SOUND_DISTANCE_THRESHOLD && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            // If the player is close enough, play the audio at the start of the animation loop
            else if (distanceToPlayer <= SOUND_DISTANCE_THRESHOLD)
            {
                if (Time.time - lastPlayTime >= animationLength)
                {
                    if (audioSource != null && stagSound != null)
                    {
                        audioSource.Play();
                        lastPlayTime = Time.time; // Update the last play time
                    }
                }
            }
        }
    }
}