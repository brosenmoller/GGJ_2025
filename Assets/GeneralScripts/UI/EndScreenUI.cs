using PrimeTween;
using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deathCountValueText;
    [SerializeField] private TextMeshProUGUI lowestDeathCountValueText;
    [SerializeField] private TextMeshProUGUI newHighScoreText;

    private bool hasBrokenHighscore;

    private void Awake()
    {
        int deathCount = PlayerPrefs.GetInt(SaveSystem.DEATHS_SAVE);
        int lowestDeathCount = PlayerPrefs.GetInt(SaveSystem.LOWEST_DEATHS_SAVE);

        if (deathCount < lowestDeathCount)
        {
            hasBrokenHighscore = true;
            lowestDeathCount = deathCount;
            PlayerPrefs.SetInt(SaveSystem.LOWEST_DEATHS_SAVE, lowestDeathCount);
        }

        PlayerPrefs.Save();

        deathCountValueText.text = deathCount.ToString();
        lowestDeathCountValueText.text = lowestDeathCount.ToString();

        if (hasBrokenHighscore)
        {
            newHighScoreText.enabled = true;
            Tween.Scale(newHighScoreText.transform, endValue: 1.2f, duration: 1f, ease: Ease.InOutSine, cycleMode: CycleMode.Rewind, cycles: 100);
        }
    }
}
