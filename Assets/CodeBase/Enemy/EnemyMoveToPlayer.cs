using System;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float speed;
        [SerializeField] private float minimalDistance = 4;

        private IGameFactory _gameFactory;
        private Transform targetTransform;

        [Inject]
        private void Init(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;

            if (_gameFactory.HeroGameObject != null)
                InitializeHeroTransform();
            else
            {
                _gameFactory.HeroCreated += InitializeHeroTransform;
            }
        }

        public void FixedUpdate()
        {
            if (targetTransform != null)
            {
                float distance = Vector2.Distance(_rb.transform.position, targetTransform.position);
                if (distance >= minimalDistance) 
                    Chase();
            }
        }

        private void Chase()
        {
            Vector2 direction = (targetTransform.position - _rb.transform.position).normalized;
            LookAtTarget(direction);
            Vector2 velocity = _rb.velocity;
            velocity.x = speed * direction.x;
            _rb.velocity = velocity;
        }

        private void LookAtTarget(Vector2 direction)
        {
            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        private void InitializeHeroTransform() => 
            targetTransform = _gameFactory.HeroGameObject.transform;
    }
}