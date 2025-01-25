using UnityEngine;

public static class TransformExtention
{
    public static Vector2 position2D(this Transform transform)
    {
        return new(transform.position.x, transform.position.y);
    }
}
