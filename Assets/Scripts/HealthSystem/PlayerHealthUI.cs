using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    public static PlayerHealthUI Instance { get; private set; }

    [SerializeField] TMPro.TMP_Text healthText;
    [SerializeField] Health playerHealth;

    void Awake()
    {
        // Configura el singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        UpdateHealthUI();
        Debug.Log("PlayerHealthUI iniciado");
    }

    public void UpdateHealthUI()
    {
        if (playerHealth != null && healthText != null)
        {
            Debug.Log("Actualizando UI de salud");
            healthText.text = "Health: " + playerHealth.health.ToString();
        }
    }
}
