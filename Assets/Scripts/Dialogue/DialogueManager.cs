using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image continueIcon;

    [Header("Dialogue Settings")]
    [SerializeField] private float continueCooldown = 0.25f;   // tiempo mínimo entre avances
    [SerializeField] private float endCooldownDuration = 1.0f; // cooldown después de terminar diálogo

    private Story currentStory;
    private bool isDialoguePlaying;
    private string currentLine;

    private float lastContinueTime = -999f;
    private float endCooldownTimer = 0f;
    private Language currentLanguage;

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
        // decrementa cooldown final
        if (endCooldownTimer > 0f)
            endCooldownTimer -= Time.deltaTime;

        if (!isDialoguePlaying) return;

        // input para avanzar
        if (InputManager.interact && Time.time - lastContinueTime >= continueCooldown)
        {
            ContinueStory();
        }
    }

    public void StartDialogue(TextAsset inkJSON, Language language)
    {
        if (!CanStartDialogue) return;

        currentLanguage = language;

        if (inkJSON == null)
        {
            Debug.LogWarning("DialogueManager: inkJSON es null.");
            return;
        }

        currentStory = new Story(inkJSON.text);
        isDialoguePlaying = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void ContinueStory()
    {
        if (currentStory == null) return;

        if (currentStory.canContinue)
        {
            currentLine = currentStory.Continue().Trim();

            // aplica traducción según nivel de idioma
            string translatedLine = LanguageSystem.Instance.TranslateLine(currentLine, currentLanguage);

            if (dialogueText != null) dialogueText.text = translatedLine;
            lastContinueTime = Time.time;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialoguePlaying = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (dialogueText != null) dialogueText.text = "";
        currentStory = null;

        // inicia cooldown post-diálogo
        endCooldownTimer = endCooldownDuration;
    }
}
