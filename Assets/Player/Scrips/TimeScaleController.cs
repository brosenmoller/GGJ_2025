using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if UNITY_EDITOR
        InputManager.Instance.Controls.Debug.MinusTimeScale.started += _ => {
            if (Time.timeScale != 0)
                Time.timeScale -= 0.1f;
        };
        InputManager.Instance.Controls.Debug.PlusTimeScale.started += _ => {
            if (Time.timeScale != 4)
                Time.timeScale += 0.1f;
        };
        InputManager.Instance.Controls.Debug.PauseTimeScale.started += _ => {
            Time.timeScale = 0;
        };
#endif
    }
}
