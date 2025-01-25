using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void SetMoving(bool b)
    {
        animator.SetBool("IsMoving",b);
    }

    public void SetGrounded(bool b)
    {
        animator.SetBool("IsGrounded", b);
    }
    public void InteractImpulse()
    {
        animator.SetTrigger("Interact");
    }
    public void JumpTrigger()
    {
        animator.SetTrigger("Jump");
    }
    public void LandTrigger()
    {
        animator.SetTrigger("Land");
    }

}
