using UnityEngine;

namespace CodeBase.Enemy
{
    public static class PhysicsDebug
    {
        public static void DrawDebug(Vector2 worldPosition, float radius, float seconds)
        {
            Debug.DrawRay(worldPosition, radius * Vector2.up, Color.red, seconds);
            Debug.DrawRay(worldPosition, radius * Vector2.left, Color.red, seconds);
            Debug.DrawRay(worldPosition, radius * Vector2.right, Color.red, seconds);
            Debug.DrawRay(worldPosition, radius * Vector2.down, Color.red, seconds);
        }

    }
}