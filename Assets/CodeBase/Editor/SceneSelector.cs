using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class SceneSelector : EditorWindow
    {
        private string selectedScene = "Initial";
        private string[] availableScenes;

        [MenuItem("Tools/Scene Launch Settings")]
        public static void ShowWindow() => 
            GetWindow<SceneSelector>("Scene Launch Settings");

        private void OnEnable() => 
            availableScenes = GetAvailableScenes();

        private void OnGUI()
        {
            GUILayout.Label("Choose Scene to Launch", EditorStyles.boldLabel);

            int selectedIndex = System.Array.IndexOf(availableScenes, selectedScene);
            selectedIndex = EditorGUILayout.Popup("Scene", selectedIndex, availableScenes);

            selectedScene = availableScenes[selectedIndex];

                if (GUILayout.Button("Set Launch Scene"))
                {
                    SetLaunchScene(selectedScene);
                    EditorApplication.isPlaying = true;
                }
        }

        private string[] GetAvailableScenes()
        {
            EditorBuildSettingsScene[] scenePaths = EditorBuildSettings.scenes;
            string[] scenes = new string[scenePaths.Length];

            for (int i = 0; i < scenePaths.Length; i++) 
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension(scenePaths[i].path);

            return scenes;
        }

        private void SetLaunchScene(string sceneName)
        {
            EditorPrefs.SetString("LaunchScene", sceneName);
            Debug.Log($"Launch scene set to: {sceneName}");
        }
    }
}