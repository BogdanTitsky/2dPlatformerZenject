using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class LootDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Counter;
        
        private WorldData _worldData;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;

        [Inject]
        public void Init(IPersistentProgressService progressService, ISaveLoadService saveLoadService, IGameFactory gameFactory)
        {
            _worldData = progressService.Progress.WorldData;
            _worldData.LootData.OnChange += UpdateCounter;
            _worldData.LootData.OnChange += Save;
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            Counter.text = $"{_worldData.LootData.Collected}";
        }

        private void Save()
        {
            _saveLoadService.SaveProgress(_gameFactory.ProgressWriters);
        }
    }
}