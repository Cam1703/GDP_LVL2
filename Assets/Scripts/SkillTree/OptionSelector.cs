using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionSelector : MonoBehaviour
{
    [Header("Botones de opciones (3 botones)")]
    [SerializeField] private Button[] optionButtons;

    private Button currentSelected;

    [Header("Construcción de Panel")]
    [SerializeField] private GameObject selectedLanguagePanel;
    [SerializeField] private GameObject selectedLanguage;
    private void Start()
    {
        foreach (Button btn in optionButtons)
        {
            // Asignar evento de click
            btn.onClick.AddListener(() => OnOptionSelected(btn));

        }
        selectedLanguagePanel.SetActive(false);
    }

    void OnHover(Button hoveredButton)
    {
        // Si ya está seleccionado, no cambia el hover
        if (currentSelected == hoveredButton)
            return;

        // Restaurar todos los botones
        foreach (Button btn in optionButtons)
            btn.transform.localScale = Vector3.one;

        // Resaltar el botón sobre el que se pasa el ratón
        hoveredButton.transform.localScale = Vector3.one * 1.2f;
        hoveredButton.transform.SetAsLastSibling();
    }

    void OnOptionSelected(Button selectedButton)
    {
        // Restaurar todos los botones al tamaño normal
        foreach (Button btn in optionButtons)
            btn.transform.localScale = Vector3.one;

        // Actualizar la referencia del botón actual
        currentSelected = selectedButton;
        
        Language selectedLanguage = (Language)System.Enum.Parse(typeof(Language), selectedButton.name);

        // Mostrar panel 
        if (selectedLanguagePanel != null)
            ShowLanguagePanel(selectedLanguage);

        Debug.Log($"Opción seleccionada: {selectedButton.name}");
    }

    void ShowLanguagePanel(Language language)
    {
        selectedLanguagePanel.SetActive(true);

    }
}
