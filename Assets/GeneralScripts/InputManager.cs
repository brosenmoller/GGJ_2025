using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public PlayerControls Controls { get; private set; }

    private void Awake()
    {
        Instance = this;
        Controls = new();
        Controls.Gameplay.Enable();
#if UNITY_EDITOR
        Controls.Debug.Enable();
#endif
    }

    private void OnDisable() 
    {
        Controls.Gameplay.Disable();
#if UNITY_EDITOR
        Controls.Debug.Disable();
#endif
    }
}
