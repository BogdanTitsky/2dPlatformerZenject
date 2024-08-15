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

        private Transform _targetTransform;
        private IGameFactory _gameFactory;

        [Inject]
        private void Init(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _targetTransform = _gameFactory.HeroGameObject.transform;
        }

        public void FixedUpdate()
        { 
            float distance = Vector2.Distance(_rb.transform.position, _targetTransform.position);
            if (distance >= minimalDistance) 
                Chase();
        }

        private void Chase()
        {
            Vector2 direction = (_targetTransform.position - _rb.transform.position).normalized;
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
    }
}