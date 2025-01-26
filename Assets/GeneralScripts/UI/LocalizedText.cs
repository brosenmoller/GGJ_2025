using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : Localized
{
    [SerializeField] private string englishText;
    [SerializeField] private string koreanText;

    private TextMeshProUGUI text;

    public override void OnLanguageChanged(int lanuage)
    {
        ApplyLanguage();
    }

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        int language = PlayerPrefs.GetInt(SaveSystem.LANGUAGE_SAVE);
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        text.text = language == SaveSystem.LANGUAGE_ENGLISH ? englishText : koreanText;
    }
}
