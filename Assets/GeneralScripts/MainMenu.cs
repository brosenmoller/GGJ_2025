using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button languageSwitchButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        languageSwitchButton.onClick.AddListener(SwitchLanguage);
        SaveSystem.Setup();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
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