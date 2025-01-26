using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deathCountValueText;
    [SerializeField] private TextMeshProUGUI lowestDeathCountValueText;
    [SerializeField] private TextMeshProUGUI timeValueText;
    [SerializeField] private TextMeshProUGUI fastestTimeValueText;
    [SerializeField] private TextMeshProUGUI newLowestDeathText;
    [SerializeField] private TextMeshProUGUI newFastestTimeText;
    [SerializeField] private Button backToMainMenu;
    [SerializeField] private Button restartButton;

    private bool hasReachedLowestScore;
    private bool hasReachedFastestTime;

    private void Awake()
    {
        int deathCount = PlayerPrefs.GetInt(SaveSystem.DEATHS_SAVE);
        int lowestDeathCount = PlayerPrefs.GetInt(SaveSystem.LOWEST_DEATHS_SAVE);

        float time = SaveSystem.EndTime();
        float fastestTime = PlayerPrefs.GetFloat(SaveSystem.FASTEST_TIME_SAVE);

        if (deathCount < lowestDeathCount)
        {
            hasReachedLowestScore = true;
            lowestDeathCount = deathCount;
            PlayerPrefs.SetInt(SaveSystem.LOWEST_DEATHS_SAVE, lowestDeathCount);
        }

        if (time < fastestTime)
        {
            hasReachedFastestTime = true;
            fastestTime = time;
            PlayerPrefs.SetFloat(SaveSystem.FASTEST_TIME_SAVE, fastestTime);
        }

        PlayerPrefs.Save();

        deathCountValueText.text = deathCount.ToString();
        lowestDeathCountValueText.text = lowestDeathCount.ToString();

        timeValueText.text = time.ToString();
        fastestTimeValueText.text = fastestTime.ToString();

        if (hasReachedLowestScore)
        {
            newLowestDeathText.enabled = true;
            Tween.Scale(newLowestDeathText.transform, endValue: 1.2f, duration: 1f, ease: Ease.InOutSine, cycleMode: CycleMode.Rewind, cycles: 100);
        }

        if (hasReachedFastestTime)
        {
            newFastestTimeText.enabled = true;
            Tween.Scale(newFastestTimeText.transform, endValue: 1.2f, duration: 1f, ease: Ease.InOutSine, cycleMode: CycleMode.Rewind, cycles: 100);
        }

        backToMainMenu.onClick.AddListener(BackToMain);
        restartButton.onClick.AddListener(Restart);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SaveSystem.StartTime();
        SceneManager.LoadScene(1);
        SaveSystem.Setup();
    }
}
