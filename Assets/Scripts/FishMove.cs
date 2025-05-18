using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;
    public float rectangleWidth = 10f;
    public float rectangleHeight = 5f;
    public float waterLevel = 0f;
    public float jumpHeight = 2f;
    public float jumpDuration = 1f;
    private float jumpIntervalMin = 3f;
    private float jumpIntervalMax = 7f;
    private float jumpInterval;

    private Vector3 centerPosition;
    private Vector3 targetPosition;
    private float timeSinceLastJump = 0f;
    private float jumpTimer = 0f;
    private bool isJumping = false;
    private AudioSource audioSource;
    private GameObject player;

    void Start()
    {
        centerPosition = transform.position;
        centerPosition.y = waterLevel;
        transform.position = centerPosition;

        jumpInterval = Random.Range(jumpIntervalMin, jumpIntervalMax);

        PickNewTarget();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        timeSinceLastJump += Time.deltaTime;

        MoveWithinRectangle();
        HandleJump();
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
        targetPosition = new Vector3(randomX, waterLevel, randomZ);
    }

    void HandleJump()
    {
        if (timeSinceLastJump >= jumpInterval && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
            timeSinceLastJump = 0f;
        }

        if (isJumping)
        {
            jumpTimer += Time.deltaTime / jumpDuration;
            float height = Mathf.Sin(jumpTimer * Mathf.PI) * jumpHeight;
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(currentPos.x, waterLevel + height, currentPos.z);

            if (jumpTimer >= 1f)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x, waterLevel, transform.position.z);
            }
        }
    }
}