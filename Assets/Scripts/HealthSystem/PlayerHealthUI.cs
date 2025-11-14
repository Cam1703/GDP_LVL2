using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public static PlayerHealthUI Instance { get; private set; }

    [SerializeField] Health playerHealth;
    [SerializeField] Sprite[] healthSprites;
    [SerializeField] Image healthImage;

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
        if (playerHealth != null && healthSprites.Length > 0)
        {
            Debug.Log("Actualizando UI de salud");
            healthImage.sprite = healthSprites[playerHealth.health];
            //healthText.text = "Health: " + playerHealth.health.ToString();
        }
    }
}
