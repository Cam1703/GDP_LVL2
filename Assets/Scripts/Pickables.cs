using UnityEngine;

public class Pickables : MonoBehaviour
{

    public GameObject inventory;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            visualCue.SetActive(true);
            // Perform action when colliding with an object tagged "Enemy"
            if (InputManager.interact)
            { 
                Debug.Log("Collided with Enemy");

                InventoryControl inv = inventory.GetComponent<InventoryControl>();
                inv.collected.Add(gameObject.name, true);
                if (inv.startDone)
                {
                    inv.UpdateList();
                }
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            visualCue.SetActive(false);
        }
    }
}
