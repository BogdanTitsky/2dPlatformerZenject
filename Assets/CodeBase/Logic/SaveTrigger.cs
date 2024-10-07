using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        public BoxCollider2D Collider;
        private IGameFactory _gameFactory;

        [Inject]
        public void Init(ISaveLoadService saveLoadService, IGameFactory gameFactory)
        {
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _saveLoadService.SaveProgress(_gameFactory.ProgressWriters);
                Debug.Log("Progress saved!");
                gameObject.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            if (!Collider) return;

            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position, Collider.size);
        }
    }
}