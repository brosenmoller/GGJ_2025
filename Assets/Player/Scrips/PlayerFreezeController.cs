using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFreezeController : MonoBehaviour
{
    private void Start()
    {
        InputManager.Instance.Controls.Gameplay.Freeze.performed += PlayerFreezeAction;
    }

    public void PlayerFreezeAction(InputAction.CallbackContext callbackContext)
    {
        BubbleController[] freezables = FindObjectsByType<BubbleController>(FindObjectsSortMode.None);
        foreach (BubbleController controller in freezables)
        { 
            controller.Freeze();
        }
    }
}
