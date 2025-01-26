using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button languageSwitchButton;
    [SerializeField] private Button achievementsButton;

    [SerializeField] private GameObject achievementsObject;
    [SerializeField] private TextMeshProUGUI lowestDeathValueText;
    [SerializeField] private TextMeshProUGUI fastestTimeValueText;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        languageSwitchButton.onClick.AddListener(SwitchLanguage);
        achievementsButton.onClick.AddListener(ToggleAchievements);
        SaveSystem.Setup();

        achievementsObject.SetActive(false);
        int lowestDeathCount = PlayerPrefs.GetInt(SaveSystem.LOWEST_DEATHS_SAVE);
        float fastestTime = PlayerPrefs.GetFloat(SaveSystem.FASTEST_TIME_SAVE);

        lowestDeathValueText.text = lowestDeathCount > 10000 ? " - " : lowestDeathCount.ToString();
        fastestTimeValueText.text = fastestTime > 100000f ? " - " : fastestTime.ToString("0.00");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SaveSystem.StartTime();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleAchievements()
    {
        achievementsObject.SetActive(!achievementsObject.activeSelf);
    }

    public void SwitchLanguage()
    {
        SaveSystem.SwitchLanguage();
    }
#if UNITY_EDITOR
    [ContextMenu("Reset Save")]
    public void ResetSave()
    {
        SaveSystem.Reset();
        Debug.Log("Save Reset!");
    }
#endif
}