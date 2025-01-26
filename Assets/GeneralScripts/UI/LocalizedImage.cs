using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LocalizedImage : Localized
{
    [SerializeField] private Sprite englishSprite;
    [SerializeField] private Sprite koreanSprite;

    private Image image;

    public override void OnLanguageChanged(int lanuage)
    {
        ApplyLanguage();
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        int language = PlayerPrefs.GetInt(SaveSystem.LANGUAGE_SAVE);
        image.sprite = language == SaveSystem.LANGUAGE_ENGLISH ? englishSprite : koreanSprite;
    }
}