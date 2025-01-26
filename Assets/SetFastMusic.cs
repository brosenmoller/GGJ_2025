using UnityEngine;

public class SetFastMusic : MonoBehaviour
{
    private void Start()
    {
        if (MusicManager.Exists) {
            MusicManager.Instance.SwitchToFast();
        }
    }
}
