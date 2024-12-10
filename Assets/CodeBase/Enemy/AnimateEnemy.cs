using System;
using CodeBase.Infrastructure.Services.Pause;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemyAnimator))]
    
    public class AnimateEnemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        
        private const float MinimalVelocity = 0.1f;

        private IPauseService _pauseService;

        [Inject]
        public void Init(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }

        private void Update()
        {
            if (ShouldMove())
                _enemyAnimator.Move();
            else
                _enemyAnimator.StopMoving();
        }

        private bool ShouldMove()
        {
            return Math.Abs(_rb.linearVelocity.x) > MinimalVelocity && !_pauseService.IsPaused;
        }
    }
}