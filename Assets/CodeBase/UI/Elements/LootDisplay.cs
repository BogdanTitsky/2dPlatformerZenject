using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LootDisplay : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private TextMeshProUGUI Counter;

        public int CurrentLoot
        {
            get => _currentLoot;
            set
            {
                _currentLoot = value;
                UpdateCounter();
            } 
        }
        private int _currentLoot;

        public void LoadProgress(PlayerProgress progress)
        {
            CurrentLoot = progress.WorldData.LootData.Collected;
            UpdateCounter();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.LootData.Collected = CurrentLoot;
        }
        
        private void UpdateCounter()
        {
            Counter.text = $"{CurrentLoot}";
        }
    }
}