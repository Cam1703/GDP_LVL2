using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    public bool playerInRange;

    [Header("Ink JSON Asset")]
    [SerializeField] private TextAsset inkJSON;

    [SerializeField] private Language language;


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
            // Mostrar cue solo si no hay diálogo activo
            visualCue.SetActive(true);

            if (InputManager.interact)
            {
                DialogueManager.Instance.StartDialogue(inkJSON, language);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
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
