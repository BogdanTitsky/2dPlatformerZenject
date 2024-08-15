using CodeBase.CameraLogic._2D_Camera.Scripts;
using UnityEditor;
using UnityEngine;

namespace CodeBase.CameraLogic._2D_Camera.Editor
{
    [CustomEditor(typeof(Rail2D))]
    [CanEditMultipleObjects]
    public class Rail2DEditor : UnityEditor.Editor
    {
    

        void OnEnable() {
        
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorList.Show(serializedObject.FindProperty("railAnchors"));
            serializedObject.ApplyModifiedProperties();
        
            GUI.color = Color.green;
            if (GUILayout.Button("Add Anchor +")) {
                Rail2D rail = ((Rail2D)target);
                RailAnchor2D anchor = rail.AddAnchor("Anchor");
                // Selection.activeGameObject = anchor.gameObject;
            }
            GUI.color = Color.white;
        }
    }
}
