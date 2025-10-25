using UnityEngine;

public class Pickables : MonoBehaviour
{

    public GameObject inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
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
}
