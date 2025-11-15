using UnityEngine;
using UnityEngine.UI;

public class PlayerXpUI : MonoBehaviour
{
    public static PlayerXpUI instance;

    [SerializeField] LanguageSystem playerXp;
    [SerializeField] Sprite[] xpSprites;
    [SerializeField] Image xpImage;

    [SerializeField] Sprite[] spSprites;
    [SerializeField] Image spImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Configura el singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        Debug.Log("PlayerXpUI iniciado");
    }

    public void UpdateXpUI()
    {

        if (playerXp != null && spSprites.Length > 0)
        {
            Debug.Log("Actualizando Skill Points");
            spImage.sprite = spSprites[playerXp.skillPoints];
            //healthText.text = "Health: " + playerHealth.health.ToString();
        }

        if (playerXp != null && xpSprites.Length > 0)
        {
            Debug.Log("Actualizando UI de experiencia");
            xpImage.sprite = xpSprites[playerXp.experiencePoints / 20];
            //healthText.text = "Health: " + playerHealth.health.ToString();
        }
    }
}
