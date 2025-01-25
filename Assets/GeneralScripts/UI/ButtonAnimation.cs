using UnityEngine;
using UnityEngine.EventSystems;
using PrimeTween;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Tween.Scale(transform, endValue: 1.2f, duration: .5f, ease: Ease.OutBounce);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tween.Scale(transform, endValue: 1f, duration: .5f, ease: Ease.OutBounce);
    }
}
