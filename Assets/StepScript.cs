using UnityEngine;

public class StepScript : MonoBehaviour
{
    public void step()
    {
        AudioManager.Instance.Step(transform.position);
    }  
}
