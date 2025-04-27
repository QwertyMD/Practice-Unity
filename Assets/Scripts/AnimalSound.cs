using UnityEngine;

public class AnimalSound : MonoBehaviour
{
    public AudioClip animalSound;      // The sound to play for this animal
    private const float SOUND_DISTANCE_THRESHOLD = 20f; // Fixed distance for sound to play
    private const float PLAY_INTERVAL = 5f; // Time between audio plays (adjustable)

    private AudioSource audioSource;  // To play the animal sound
    private GameObject player;        // Reference to the player
    private float lastPlayTime = -10f; // Track when the sound was last played (negative to play immediately)

    void Start()
    {
        // Add an AudioSource component to this animal if it doesn't have one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = animalSound;
        audioSource.playOnAwake = false;
        audioSource.loop = true; // Loop the audio

        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player object has the 'Player' tag.");
        }
    }

    void Update()
    {
        // Check distance to the player
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // If the player is too far, stop the audio if it's playing
            if (distanceToPlayer > SOUND_DISTANCE_THRESHOLD)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                    lastPlayTime = Time.time; // Reset the timer when stopping
                }
            }
            // If the player is close enough, play the audio on a loop
            else if (distanceToPlayer <= SOUND_DISTANCE_THRESHOLD)
            {
                if (!audioSource.isPlaying && Time.time - lastPlayTime >= PLAY_INTERVAL)
                {
                    if (audioSource != null && animalSound != null)
                    {
                        audioSource.Play();
                        lastPlayTime = Time.time; // Update the last play time
                    }
                }
            }
        }
    }
}