using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private Image continueIcon;

    [Header("Dialogue Settings")]
    [SerializeField] private float continueCooldown = 0.25f;   // time between skips
    [SerializeField] private float endCooldownDuration = 1.0f; // cooldown after dialogue ends

    private Story currentStory;
    private bool isDialoguePlaying;
    private string currentLine;

    private float lastContinueTime = -999f;
    private float endCooldownTimer = 0f;

    public bool IsDialoguePlaying => isDialoguePlaying;
    public bool CanStartDialogue => !isDialoguePlaying && endCooldownTimer <= 0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<DialogueManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("DialogueManager");
                    instance = obj.AddComponent<DialogueManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        isDialoguePlaying = false;
    }

    private void Update()
    {
        // Handle post-dialogue cooldown
        if (endCooldownTimer > 0f)
            endCooldownTimer -= Time.deltaTime;

        if (!isDialoguePlaying) return;

        // Input for advancing dialogue
        if (InputManager.interact && Time.time - lastContinueTime >= continueCooldown)
        {
            ContinueStory();
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if (!CanStartDialogue) return;

        if (inkJSON == null)
        {
            Debug.LogWarning("DialogueManager: inkJSON is null.");
            return;
        }

        currentStory = new Story(inkJSON.text);
        isDialoguePlaying = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void ContinueStory()
    {
        if (currentStory == null) return;

        if (currentStory.canContinue)
        {
            currentLine = currentStory.Continue().Trim();

            // Handle Ink tags (like #speaker:Name, #portrait:SpriteName)
            HandleTags(currentStory.currentTags);

            if (dialogueText != null)
                dialogueText.text = currentLine;

            lastContinueTime = Time.time;
        }
        else
        {
            EndDialogue();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) continue;

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case "speaker":
                    if (speakerNameText != null)
                        speakerNameText.text = tagValue;
                    break;

                case "portrait":
                    if (portraitImage != null)
                    {
                        // Load portrait sprite dynamically from Resources/Portraits
                        Sprite loaded = Resources.Load<Sprite>($"Portraits/{tagValue}");
                        if (loaded != null)
                        {
                            portraitImage.sprite = loaded;
                            portraitImage.enabled = true;
                        }
                        else
                        {
                            Debug.LogWarning($"DialogueManager: Portrait sprite '{tagValue}' not found in Resources/Portraits.");
                        }
                    }
                    break;

                default:
                    Debug.Log($"DialogueManager: Unknown tag '{tagKey}:{tagValue}'");
                    break;
            }
        }
    }

    private void EndDialogue()
    {
        isDialoguePlaying = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";

        if (speakerNameText != null)
            speakerNameText.text = "";

        currentStory = null;
        endCooldownTimer = endCooldownDuration;
    }
}
