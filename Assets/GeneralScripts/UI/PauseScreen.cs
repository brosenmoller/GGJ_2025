using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject pauseScreenObjects;

    public bool IsPaused;

    private void Awake()
    {
        pauseScreenObjects.SetActive(false);
        IsPaused = false;

        continueButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(Restart);
        mainMenuButton.onClick.AddListener(BackToMain);
    }

    private void Start()
    {
        InputManager.Instance.Controls.Gameplay.Pause.performed += PauseAction;
    }

    private void OnDisable()
    {
        InputManager.Instance.Controls.Gameplay.Pause.performed -= PauseAction;
    }

    public void PauseAction(InputAction.CallbackContext callbackContext)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0.0f : 1.0f;
        pauseScreenObjects.SetActive(IsPaused);
    }

    public void Restart()
    {
        TogglePause();
        SaveSystem.StartTime();
        SaveSystem.Setup();
        SceneManager.LoadScene(1);
    }

    public void BackToMain()
    {
        TogglePause();
        SceneManager.LoadScene(0);
    }
}
