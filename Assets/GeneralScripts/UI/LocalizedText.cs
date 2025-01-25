using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string englishText;
    [SerializeField] private string koreanText;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (!PlayerPrefs.HasKey(nameof(Language)))
        {
            PlayerPrefs.SetInt(nameof(Language), (int)Language.English);
        }

        Language language = (Language)PlayerPrefs.GetInt(nameof(Language));
        text.text = language == Language.English ? englishText : koreanText;
    }

    public enum Language
    {
        English = 0,
        Korean = 1,
    }
}
