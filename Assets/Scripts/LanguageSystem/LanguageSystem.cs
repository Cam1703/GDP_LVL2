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
    public int skillPoints;
    public int experiencePoints;
}

public class LanguageSystem : MonoBehaviour
{
    public static LanguageSystem Instance;

    private Dictionary<Language, int> languageLevels = new Dictionary<Language, int>();
    private const string SaveKey = "LanguageSave";

    private int skillPoints = 0;
    private int experiencePoints = 0;

    // Experiencia necesaria para ganar 1 skill point 
    private const int ExpPerSkillPoint = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
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

        skillPoints = 2;
        experiencePoints = 10;
    }

    // ------------------ NIVEL DE IDIOMAS ------------------

    public void SetLanguageLevel(Language language, int level)
    {
        languageLevels[language] = level;
        SaveData();
    }

    public void IncreaseLevel(Language language)
    {
        languageLevels[language] = GetLanguageLevel(language) + 1;
        SaveData();
    }

    public int GetLanguageLevel(Language language)
    {
        return languageLevels.ContainsKey(language) ? languageLevels[language] : 0;
    }

    // ------------------ SKILL POINTS Y EXPERIENCIA ------------------

    public int GetSkillPoints() => skillPoints;
    public int GetExperience() => experiencePoints;

    public void AddExperience(int amount)
    {
        experiencePoints += amount;
        Debug.Log($"Ganaste {amount} XP. Total: {experiencePoints}");

        // Cada vez que se supera el umbral, ganas un skill point
        while (experiencePoints >= ExpPerSkillPoint)
        {
            experiencePoints -= ExpPerSkillPoint;
            skillPoints++;
            Debug.Log($"¡Ganaste un Skill Point! Total: {skillPoints}");
        }

        SaveData();
    }

    public bool SpendSkillPoint()
    {
        if (skillPoints <= 0)
        {
            Debug.LogWarning("No tienes suficientes Skill Points.");
            return false;
        }

        skillPoints--;
        SaveData();
        return true;
    }

    public int GetExperiencePerSkillPoints()
    {
        return ExpPerSkillPoint;
    }

    // ------------------ GUARDADO ------------------

    private void SaveData()
    {
        LanguageSave save = new LanguageSave();
        foreach (var pair in languageLevels)
        {
            save.languages.Add(new LanguageData { language = pair.Key, level = pair.Value });
        }

        save.skillPoints = skillPoints;
        save.experiencePoints = experiencePoints;

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

            skillPoints = save.skillPoints;
            experiencePoints = save.experiencePoints;
        }
        else
        {
            foreach (Language lang in System.Enum.GetValues(typeof(Language)))
            {
                languageLevels[lang] = 0;
            }
            skillPoints = 0;
            experiencePoints = 0;
            SaveData();
        }
    }
}
