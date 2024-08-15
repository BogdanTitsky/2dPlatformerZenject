using CodeBase.Logic;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(SaveTriggerPoint))]
    public class SpawnPointEditor: UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SaveTriggerPoint point, GizmoType gizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(point.transform.position ,new Vector3(3.3f,3.3f, 0));
        }
    }
}