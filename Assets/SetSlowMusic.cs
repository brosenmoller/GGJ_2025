using UnityEngine;

public class SetSlowMusic: MonoBehaviour
{
    private void Start()
    {
        if (MusicManager.Exists) {
            MusicManager.Instance.SwitchToSlow();
        }
    }
}
