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
