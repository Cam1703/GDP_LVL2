using UnityEngine;
using System.Collections.Generic;

public enum Language
{
    Language1,
    Language2,
    Language3
}

[System.Serializable]
public class LanguageData
{
    public Language language;
    public int level;
}

[System.Serializable]
public class LanguageSave
{
    public List<LanguageData> languages = new List<LanguageData>();
}

public class LanguageSystem : MonoBehaviour
{
    public static LanguageSystem Instance;
    private Dictionary<Language, int> languageLevels = new Dictionary<Language, int>(); // e.g., {"Idioma 1": 2, "Idioma 2": 1} guarda el idioma y el nivel
    private const string SaveKey = "LanguageSave"; // clave para almacenar los datos en PlayerPrefs

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData(); // al iniciar, cargamos los datos guardados
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        languageLevels[Language.Language1]= 0;
        languageLevels[Language.Language2] = 1;
        languageLevels[Language.Language3] = 2;
    }


    public void SetLanguageLevel(Language language, int level)
    {
        languageLevels[language] = level;
        SaveData(); // guardamos al modificar
    }

    public void IncreaseLevel(Language language)
    {
        languageLevels[language] = GetLanguageLevel(language) + 1;
        SaveData(); // guardamos al subir de nivel
    }

    public int GetLanguageLevel(Language language)
    {
        return languageLevels.ContainsKey(language) ? languageLevels[language] : 0;
    }

    /*
        El TranslateLine toma una línea en el formato "Idioma|Texto" y devuelve el texto traducido
        según el nivel de conocimiento del idioma del jugador.

        Niveles de conocimiento:
        0 - No entiende nada: "??? ??? ???"
        1 - Entiende un poco: muestra el primer tercio del texto seguido de "..."
        2 - Entiende bastante: muestra los dos tercios del texto seguido de "..."
        3 - Entiende todo: muestra el texto completo

        Ejemplo:
        Input: "Idioma 1|Hola, ¿cómo estás?"
        Nivel de "Idioma 1": 2
        Output: "Hola, ¿cómo e..."

        Voy a ir mejorando este algoritmo con el tiempo, pero la idea es que el codigo se encargue de 
        crear la sensación de que el jugador no entiende el idioma sin necesidad de crear múltiples versiones
        de cada línea de diálogo.
    */

    public string TranslateLine(string line, Language language)
    {
        int level = GetLanguageLevel(language);

        if (level <= 0) return GetObfuscatedText(line);
        if (level == 1) return line.Substring(0, Mathf.Min(line.Length / 3, line.Length)) + "...";
        if (level == 2) return line;
        return line;
    }

    private string GetObfuscatedText(string text)
    {
        // Reemplaza cada carácter diferente de espacio con un símbolo de interrogación

        string obfuscated = "";
        foreach (char c in text)
        {
            if (char.IsWhiteSpace(c))
                obfuscated += c; // mantiene los espacios
            else
                obfuscated += "as"; // reemplaza otros caracteres
        }
        return obfuscated;
    }

    // --- Métodos de guardado y carga ---

    private void SaveData()
    {
        LanguageSave save = new LanguageSave();
        foreach (var pair in languageLevels)
        {
            save.languages.Add(new LanguageData { language = pair.Key, level = pair.Value });
        }

        string json = JsonUtility.ToJson(save);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            LanguageSave save = JsonUtility.FromJson<LanguageSave>(json);

            languageLevels.Clear();
            foreach (var data in save.languages)
            {
                languageLevels[data.language] = data.level;
            }
        }
        else
        {
            // Inicializa todos los idiomas en nivel 0 si no hay datos guardados
            foreach (Language lang in System.Enum.GetValues(typeof(Language)))
            {
                languageLevels[lang] = 0;
            }
            SaveData();
        }
    }
}
