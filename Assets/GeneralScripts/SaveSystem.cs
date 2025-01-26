using UnityEngine;

public static class SaveSystem
{
    public const string DEATHS_SAVE = "Deaths";
    public const string LOWEST_DEATHS_SAVE = "LowestDeaths";
    public const string START_TIME = "STATE_TIME";
    public const string FASTEST_TIME_SAVE = "FASTEST_TIME";
    public const string LANGUAGE_SAVE = "language";

    public const int LANGUAGE_ENGLISH = 0;
    public const int LANGUAGE_KOREAN = 1;
    
    public static void Setup()
    {
        if (!PlayerPrefs.HasKey(LANGUAGE_SAVE))
        {
            SetLanguage(LANGUAGE_ENGLISH);
        }
        else 
        {
            SetLanguage(PlayerPrefs.GetInt(LANGUAGE_SAVE));
        }

        PlayerPrefs.SetInt(DEATHS_SAVE, 0);

        if (!PlayerPrefs.HasKey(LOWEST_DEATHS_SAVE))
        {
            PlayerPrefs.SetInt(LOWEST_DEATHS_SAVE, int.MaxValue);
        }

        PlayerPrefs.SetFloat(START_TIME, 0);

        if (!PlayerPrefs.HasKey(FASTEST_TIME_SAVE))
        {
            PlayerPrefs.SetFloat(FASTEST_TIME_SAVE, float.MaxValue);
        }

        PlayerPrefs.Save();
    }

    public static void StartTime()
    {
        PlayerPrefs.SetFloat(START_TIME, Time.realtimeSinceStartup);
    }

    public static float EndTime()
    {
        float startTime = PlayerPrefs.GetFloat(START_TIME);
        return Time.realtimeSinceStartup - startTime;
    }

    public static void SwitchLanguage()
    {
        SetLanguage(PlayerPrefs.GetInt(LANGUAGE_SAVE) == LANGUAGE_ENGLISH ? LANGUAGE_KOREAN : LANGUAGE_ENGLISH);
    }

    public static void SetLanguage(int language)
    {
        PlayerPrefs.SetInt(LANGUAGE_SAVE, language);
        PlayerPrefs.Save();

        Localized[] localizedObjects = Object.FindObjectsByType<Localized>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Localized localizedObject in localizedObjects)
        {
            localizedObject.OnLanguageChanged(language);
        }
    }

    public static void Reset()
    {
        PlayerPrefs.SetInt(LOWEST_DEATHS_SAVE, int.MaxValue);
        PlayerPrefs.SetFloat(FASTEST_TIME_SAVE, float.MaxValue);
    }
}