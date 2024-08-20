using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class LootCollector : MonoBehaviour
    {
        [SerializeField] private GameObject coin;
        [SerializeField] private GameObject fxPrefab;
        [SerializeField] private TextMeshPro lootText;
        [SerializeField] private GameObject pickupPopup;
        
        private Loot _loot;
        private bool _isPickedUp;
        private WorldData _worldData;

        [Inject]
        public void Init(IPersistentProgressService progressService) => 
            _worldData = progressService.Progress.WorldData;

        public void InitLootItem(Loot loot) => 
            _loot = loot;

        private void OnTriggerEnter2D(Collider2D other) => Pickup();

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
            coin.SetActive(false);

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