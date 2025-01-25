using UnityEngine;

public static class Extensions
{
    public static bool Approx(this Vector2 a, Vector2 b) => Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);

    public static bool IsInLayerMask(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;
    public static bool IsInLayerMask(GameObject obj, LayerMask mask) => IsInLayerMask(obj.layer, mask);
    public static bool Contains(this LayerMask mask, int layer) => IsInLayerMask(layer, mask);
    public static bool Contains(this LayerMask mask, GameObject obj) => IsInLayerMask(obj, mask);
}