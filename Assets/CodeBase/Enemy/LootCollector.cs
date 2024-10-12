using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class LootCollector : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private GameObject collectableSprite;
        [SerializeField] private GameObject fxPrefab;
        [SerializeField] private TextMeshPro lootText;
        [SerializeField] private GameObject pickupPopup;
        
        private Loot _loot;
        private bool _isPickedUp;
        private WorldData _worldData;

        [Inject]
        public void Init(IPersistentProgressService progressService) => 
            _worldData = progressService.Progress.WorldData;

        public void InitLootItem(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter2D(Collider2D other) => Pickup();

        public void LoadProgress(PlayerProgress progress)
        {
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            Loot lootInList = FindThisLootInData(progress);
            if (!_isPickedUp && lootInList == null)
                SaveUncollectedLoot(progress);
            else if (_isPickedUp && lootInList != null) 
                RemoveUncollectedLoot(progress, lootInList);
        }

        private Loot FindThisLootInData(PlayerProgress progress)
        {
            Loot lootInList = progress.WorldData.NotCollectedLoot.NotCollectedList.Find(loot => loot.Id == _loot.Id);
            return lootInList;
        }

        private static void RemoveUncollectedLoot(PlayerProgress progress, Loot lootInList)
        {
            progress.WorldData.NotCollectedLoot.NotCollectedList.Remove(lootInList);
        }

        private void SaveUncollectedLoot(PlayerProgress progress)
        {
            progress.WorldData.NotCollectedLoot.NotCollectedList.Add(_loot);
        }

        private void Pickup()
        {
            if (_isPickedUp)
                return;
            
            _isPickedUp = true;
            UpdateWorldData();
            HideCoin();
            PlayPickupFx();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideCoin() => 
            collectableSprite.SetActive(false);

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        private void PlayPickupFx() => 
            Instantiate(fxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            lootText.text = $"{_loot.Value}";
            pickupPopup.SetActive(true);
        }
    }
}