using System.Linq;
using CodeBase.Infrastructure.Services.StaticData.Data;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawner;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                levelData.EnemySpawnerData = FindObjectsByType<SpawnMarker>(FindObjectsSortMode.None)
                    .Select(x =>
                        new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.EnemyTypeId, x.transform.position))
                    .ToList();

                levelData.LevelKey = SceneManager.GetActiveScene().name;

                levelData.InitialHeroPosition = GameObject.FindWithTag(InitialPointTag).transform.position;
            }
            
            EditorUtility.SetDirty(target);
        }
    }
}