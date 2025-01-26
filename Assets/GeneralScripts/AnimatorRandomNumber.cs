using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatorRandomNumber : MonoBehaviour
{
    Animator animator;
    float timeToUpdate;
    [SerializeField]
    float UpdateDelay = 1;
    void Start()
    {
        animator = GetComponent<Animator>();
    }



    private void Update()
    {
        if(Time.time > timeToUpdate)
        {
            animator.SetFloat("Random", Random.Range(0, 1001));
            timeToUpdate = Time.time + UpdateDelay;
        }
    }


}
