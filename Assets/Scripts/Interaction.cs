using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Transform playerHead;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public UnityEngine.UI.Image characterImage;
    public Button ePrompt;
    public float interactionDistance = 10f;
    public float textSpeed = 0.05f;
    public GameObject music;

    private GameObject currentInteractable;
    private bool isDialogueActive = false;
    private bool isEPromptActive = false;
    private Coroutine typewriterCoroutine;
    private Animator playerAnimator;
    private bool isPlayerStopped = false;
    private AudioSource currentAudioSource;
    private AudioSource bgMusicSource;

    void Start()
    {
        Canvas canvas = dialoguePanel.GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if (dialoguePanel == null || dialogueText == null || characterImage == null || ePrompt == null || playerHead == null || music == null)
        {
            return;
        }

        playerAnimator = GetComponentInChildren<Animator>();
        if (playerAnimator == null)
        {
            return;
        }

        bgMusicSource = music.GetComponent<AudioSource>();
        if (bgMusicSource == null)
        {
            bgMusicSource = music.AddComponent<AudioSource>();
            bgMusicSource.clip = Resources.Load<AudioClip>("background");
            bgMusicSource.playOnAwake = true;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }

        dialoguePanel.SetActive(false);
        ePrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        GameObject closestInteractable = FindClosestInteractable();

        if (closestInteractable != null)
        {
            Vector3 playerPos = playerHead != null ? playerHead.position : transform.position;
            Vector3 interactablePos = closestInteractable.transform.position;
            float distance = Vector3.Distance(playerPos, interactablePos);

            if (distance <= interactionDistance)
            {
                if (!isEPromptActive)
                {
                    ShowEPrompt(true);
                }

                if (playerHead != null)
                {
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(playerHead.position);
                    ePrompt.transform.position = screenPoint;
                }

                if (currentInteractable != closestInteractable)
                {
                    currentInteractable = closestInteractable;

                    NPCMovement npcScript = currentInteractable.GetComponent<NPCMovement>();
                    if (npcScript != null)
                    {
                        npcScript.SetMoving(false);
                    }

                    StopPlayer();
                }
            }
            else
            {
                if (currentInteractable != null && isDialogueActive)
                {
                    CloseDialogue();
                }
                else if (currentInteractable != null)
                {
                    NPCMovement npcScript = currentInteractable.GetComponent<NPCMovement>();
                    if (npcScript != null)
                    {
                        npcScript.SetMoving(true);
                    }
                    ResumePlayer();
                    ShowEPrompt(false);
                    currentInteractable = null;
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                NPCMovement npcScript = currentInteractable.GetComponent<NPCMovement>();
                if (npcScript != null)
                {
                    npcScript.SetMoving(true);
                }
                ResumePlayer();
                ShowEPrompt(false);
                currentInteractable = null;
                if (isDialogueActive)
                {
                    CloseDialogue();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            InteractableData data = currentInteractable.GetComponent<InteractableData>();
            if (data != null)
            {
                if (!isDialogueActive)
                {
                    StartDialogue(data.image, data.description, data.audioClip);
                }
                else if (typewriterCoroutine != null)
                {
                    StopCoroutine(typewriterCoroutine);
                    dialogueText.text = data.description;
                    typewriterCoroutine = null;
                }
                else
                {
                    CloseDialogue();
                }
            }
        }
    }

    GameObject FindClosestInteractable()
    {
        GameObject[] animalWalls = GameObject.FindGameObjectsWithTag("AnimalWall");
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject animalWall in animalWalls)
        {
            Vector3 playerPos = playerHead != null ? playerHead.position : transform.position;
            float distance = Vector3.Distance(playerPos, animalWall.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = animalWall;
            }
        }

        foreach (GameObject npc in npcs)
        {
            Vector3 playerPos = playerHead != null ? playerHead.position : transform.position;
            float distance = Vector3.Distance(playerPos, npc.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = npc;
            }
        }

        return closest;
    }

    void ShowEPrompt(bool show)
    {
        isEPromptActive = show;
        ePrompt.gameObject.SetActive(show);
        if (show && playerHead != null)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(playerHead.position);
            ePrompt.transform.position = screenPoint;
        }
    }

    void StopPlayer()
    {
        isPlayerStopped = true;
    }

    void ResumePlayer()
    {
        isPlayerStopped = false;
    }

    void StartDialogue(Sprite image, string text, AudioClip audioClip)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        characterImage.sprite = image;

        StopPlayer();

        if (audioClip != null)
        {
            if (bgMusicSource != null && bgMusicSource.isPlaying)
            {
                bgMusicSource.Pause();
            }

            if (currentAudioSource == null)
            {
                currentAudioSource = gameObject.AddComponent<AudioSource>();
            }
            currentAudioSource.clip = audioClip;
            currentAudioSource.playOnAwake = false;
            currentAudioSource.loop = true;
            currentAudioSource.Play();
        }

        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = StartCoroutine(TypewriterEffect(text));
    }

    void CloseDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = null;

        ResumePlayer();
        StopAudio();

        if (bgMusicSource != null && !bgMusicSource.isPlaying)
        {
            bgMusicSource.Play();
        }
    }

    void StopAudio()
    {
        if (currentAudioSource != null && currentAudioSource.isPlaying)
        {
            currentAudioSource.Stop();
        }
    }

    IEnumerator TypewriterEffect(string text)
    {
        dialogueText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }
        typewriterCoroutine = null;
    }

    void LateUpdate()
    {
        if (isPlayerStopped)
        {
        }
    }
}