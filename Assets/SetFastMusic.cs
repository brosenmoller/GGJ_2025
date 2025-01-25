using UnityEngine;

public class SetFastMusic : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.SwitchToFast();
    }
}
