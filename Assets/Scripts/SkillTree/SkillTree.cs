using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillTree : MonoBehaviour
{
    private LanguageSystem languageSystem;

    [Header("Skill Points and Experience")]
    [SerializeField] private TMP_Text skillPointsText;
    [SerializeField] private TMP_Text experienceText;

    [Header("Botones de opciones (3 botones)")]
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private Sprite[] optionButtonsImage;
    private Button currentSelected;

    [Header("Construcción de Panel")]
    [SerializeField] private GameObject selectedLanguagePanel;
    [SerializeField] private Button closePanelButton;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private GameObject[] selectedLanguageLevel;
    [SerializeField] private GameObject selectedLanguageIcon;

    private Language currentLanguage;

    private void Start()
    {
        languageSystem = LanguageSystem.Instance;

        foreach (Button btn in optionButtons)
        {
            Button capturedBtn = btn;
            capturedBtn.onClick.AddListener(() => OnOptionSelected(capturedBtn));
        }

        if (closePanelButton != null)
            closePanelButton.onClick.AddListener(DisableShowLanguagePanel);

        if (levelUpButton != null)
            levelUpButton.onClick.AddListener(OnLevelUpPressed);

        selectedLanguagePanel.SetActive(false);

        UpdateUI();
    }

    void OnOptionSelected(Button selectedButton)
    {
        foreach (Button btn in optionButtons)
            btn.transform.localScale = Vector3.one;

        currentSelected = selectedButton;
        currentLanguage = (Language)System.Enum.Parse(typeof(Language), selectedButton.name);

        ShowLanguagePanel(currentLanguage);
        Debug.Log($"Opción seleccionada: {selectedButton.name}");
    }

    void ShowLanguagePanel(Language language)
    {
        foreach (Button btn in optionButtons)
            btn.interactable = false;
        EventSystem.current.SetSelectedGameObject(closePanelButton.gameObject);
        MenuManager.instance.buttons.SetActive(false);


        selectedLanguagePanel.SetActive(true);
        ShowCurrentLevel(language);
        ShowCurrentLanguage(language);
    }

    void DisableShowLanguagePanel()
    {
        MenuManager.instance.buttons.SetActive(true);
        foreach (Button btn in optionButtons)
            btn.interactable = true;
        EventSystem.current.SetSelectedGameObject(currentSelected.gameObject);
        selectedLanguagePanel.SetActive(false);
    }

    void OnLevelUpPressed()
    {
        if (currentSelected == null)
        {
            Debug.LogWarning("No language selected to level up.");
            return;
        }

        // Requiere un skill point
        if (!languageSystem.SpendSkillPoint())
            return;

        LevelUp(currentLanguage);
    }

    void LevelUp(Language language)
    {
        if (languageSystem.GetLanguageLevel(language) >= selectedLanguageLevel.Length)
        {
            Debug.Log($"El nivel máximo para {language} ya ha sido alcanzado.");
            return;
        }

        languageSystem.IncreaseLevel(language);
        Debug.Log($"Nivel aumentado para {language}. Nuevo nivel: {languageSystem.GetLanguageLevel(language)}");
        ShowCurrentLevel(language);
        UpdateUI();
    }

    void ShowCurrentLevel(Language language)
    {
        int level = languageSystem.GetLanguageLevel(language);

        foreach (GameObject levelObj in selectedLanguageLevel)
            levelObj.SetActive(false);

        for (int i = 0; i < level && i < selectedLanguageLevel.Length; i++)
            selectedLanguageLevel[i].SetActive(true);
    }

    void ShowCurrentLanguage(Language language)
    {
        Image iconImage = selectedLanguageIcon.GetComponent<Image>();
        switch (language)
        {
            case Language.Language1:
                iconImage.sprite = optionButtonsImage[0];
                break;
            case Language.Language2:
                iconImage.sprite = optionButtonsImage[1];
                break;
            case Language.Language3:
                iconImage.sprite = optionButtonsImage[2];
                break;
        }
    }

    void UpdateUI()
    {
        skillPointsText.text = $"Skill Points: {languageSystem.GetSkillPoints()}";
        experienceText.text = $"XP: {languageSystem.GetExperience()} / {languageSystem.GetExperiencePerSkillPoints()}" ;
    }

    // --- Ejemplo: añadir experiencia desde otro lugar ---
    public void GainExperienceExample()
    {
        languageSystem.AddExperience(50);
        UpdateUI();
    }
}
