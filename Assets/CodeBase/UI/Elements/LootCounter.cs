using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class LootCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Counter;
        
        private WorldData _worldData;

        [Inject]
        public void Init(IPersistentProgressService progressService)
        {
            _worldData = progressService.Progress.WorldData;
            _worldData.LootData.OnChange += UpdateCounter;
        }

        private void Start()
        {
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            Counter.text = $"{_worldData.LootData.Collected}";
        }
    }
}