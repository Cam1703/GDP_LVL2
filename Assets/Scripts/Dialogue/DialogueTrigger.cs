using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    public bool playerInRange;

    [Header("Dialogue Options")]
    [Tooltip("Assign Ink JSONs for each level (e.g., Dialogue_Level0, Dialogue_Level1, Dialogue_Level2...)")]
    [SerializeField] private TextAsset[] dialogueOptions;

    [Header("Language Settings")]
    [SerializeField] private Language language; // Assigned manually in inspector

    [Header("Character")]
    [SerializeField] private Sprite[] portraits;
    [SerializeField] private string characterName;

    private void Awake()
    {
        if (visualCue != null)
            visualCue.SetActive(false);
        else
            Debug.LogWarning("Visual Cue is not assigned in the inspector.");

        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.Instance.IsDialoguePlaying)
        {
            visualCue.SetActive(true);

            if (InputManager.interact)
            {
                TextAsset inkJSON = GetDialogueForLevel(language);
                DialogueManager.Instance.StartDialogue(inkJSON);
            }
        }
        else
        {
            if (visualCue != null)
                visualCue.SetActive(false);
        }
    }

    private TextAsset GetDialogueForLevel(Language lang)
    {
        int level = LanguageSystem.Instance.GetLanguageLevel(lang);

        // Try to find an exact level file (e.g., Dialogue_Level2)
        foreach (var dialogue in dialogueOptions)
        {
            if (dialogue.name.EndsWith($"Level{level}"))
                return dialogue;
        }

        // Fallback: try lower levels
        for (int i = level - 1; i >= 0; i--)
        {
            foreach (var dialogue in dialogueOptions)
            {
                if (dialogue.name.EndsWith($"Level{i}"))
                    return dialogue;
            }
        }

        // Default fallback
        Debug.LogWarning($"No dialogue found for {lang} Level {level}. Returning first dialogue as fallback.");
        return dialogueOptions.Length > 0 ? dialogueOptions[0] : null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (visualCue != null)
                visualCue.SetActive(false);
        }
    }
}
