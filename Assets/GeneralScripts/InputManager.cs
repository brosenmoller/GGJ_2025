using UnityEditor.ShaderGraph;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public PlayerControls Controls { get; private set; }

    private void Awake() {
        Instance = this;
        Controls = new();
        Controls.Gameplay.Enable();
    }

    private void OnDisable() {
        Controls.Gameplay.Disable();
    }
}
