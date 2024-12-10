using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI.Elements;
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
        private LootDisplay _lootDisplay;

        [Inject]
        public void Init(LootDisplay lootDisplay)
        { 
            _lootDisplay = lootDisplay;
        }

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
            _lootDisplay.CurrentLoot += _loot.Value;
            HideCoin();
            PlayPickupFx();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void HideCoin() => 
            collectableSprite.SetActive(false);

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            if (gameObject != null) 
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