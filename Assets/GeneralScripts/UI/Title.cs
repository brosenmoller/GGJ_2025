using PrimeTween;
using UnityEngine;

public class Title : MonoBehaviour
{
    private void Start()
    {
        Tween.Scale(transform, endValue: 1.1f, duration: 5, Ease.InOutSine, cycleMode: CycleMode.Rewind);
        Tween.PositionY(transform, endValue: transform.position.y + 3, duration: 5, Ease.InOutSine, cycleMode: CycleMode.Rewind);
    }
}
