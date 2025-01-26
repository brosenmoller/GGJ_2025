using UnityEngine;
using UnityEngine.UI;

public class LocalizedImage : Localized
{
    [SerializeField] private Image englishImage;
    [SerializeField] private Image koreanImage;

    public override void OnLanguageChanged(int lanuage)
    {
        ApplyLanguage();
    }

    private void Start()
    {
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        int language = PlayerPrefs.GetInt(SaveSystem.LANGUAGE_SAVE);
        if (language == SaveSystem.LANGUAGE_KOREAN)
        {
            koreanImage.enabled = true;
            englishImage.enabled = false;
        }
        else
        {
            englishImage.enabled = true;
            koreanImage.enabled = false;
        }
    }
}