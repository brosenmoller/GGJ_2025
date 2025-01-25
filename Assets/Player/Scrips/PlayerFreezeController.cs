using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFreezeController : MonoBehaviour
{
    private PlayerAnimator animator;
    private void Start()
    {
        InputManager.Instance.Controls.Gameplay.Freeze.performed += PlayerFreezeAction;
        animator = GetComponent<PlayerAnimator>();
    }

    public void PlayerFreezeAction(InputAction.CallbackContext callbackContext)
    {
        ParticleManager.Instance.PlayParticleAt("FreezeStart", transform.position);
        AudioManager.Instance.PlayOneShotRandomPitchFromDictonary("FreezeAbility", transform.position, true);
        animator.InteractImpulse();
        BubbleController[] freezables = FindObjectsByType<BubbleController>(FindObjectsSortMode.None);
        foreach (BubbleController controller in freezables)
        { 
            controller.TriggerFreezeAction();
        }
    }
}
