using UnityEngine;

public class TicketInteraction : MonoBehaviour
{
    public Canvas ticketCanvas;
    public GameObject doorPivot1;
    public GameObject doorPivot2;
    public AudioClip cashSound;
    public float doorOpenAngle1 = 90f;
    public float doorOpenAngle2 = 90f;
    public float doorTransitionTime = 1f;

    private Camera playerCamera;
    private AudioSource audioSource;
    private bool hasOpenedDoors = false;

    void Start()
    {
        playerCamera = GetComponent<Camera>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = cashSound;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !hasOpenedDoors)
        {
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Canvas hitCanvas = hit.collider.GetComponentInParent<Canvas>();
                if (hitCanvas != null && hitCanvas == ticketCanvas)
                {
                    audioSource.Play();
                    StartCoroutine(OpenDoors());
                    hasOpenedDoors = true;
                    ticketCanvas.gameObject.SetActive(false);
                }
            }
        }
    }

    System.Collections.IEnumerator OpenDoors()
    {
        float elapsedTime = 0f;
        Quaternion startRotation1 = doorPivot1.transform.rotation;
        Quaternion startRotation2 = doorPivot2.transform.rotation;
        Quaternion endRotation1 = startRotation1 * Quaternion.Euler(0, doorOpenAngle1, 0);
        Quaternion endRotation2 = startRotation2 * Quaternion.Euler(0, doorOpenAngle2, 0);

        while (elapsedTime < doorTransitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / doorTransitionTime;
            doorPivot1.transform.rotation = Quaternion.Lerp(startRotation1, endRotation1, t);
            doorPivot2.transform.rotation = Quaternion.Lerp(startRotation2, endRotation2, t);
            yield return null;
        }

        doorPivot1.transform.rotation = endRotation1;
        doorPivot2.transform.rotation = endRotation2;
    }
}