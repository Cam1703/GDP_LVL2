using UnityEngine;

public class ShowVisualCue : MonoBehaviour
{
    [SerializeField] GameObject visualCue;
    public bool playerInRange;

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
        if (playerInRange)
        {
            visualCue.SetActive(true);
        }
        else
        {
            if (visualCue != null)
                visualCue.SetActive(false);
        }
    }


}
