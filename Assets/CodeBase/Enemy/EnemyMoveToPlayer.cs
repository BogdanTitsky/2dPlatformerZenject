﻿using System;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Pause;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float speed;
        [SerializeField] private float minimalDistance = 4;
        [SerializeField] private EnemyAnimator animator;
        
        private HeroDeath hero;
        private Transform _targetTransform;
        private IGameFactory _gameFactory;
        public bool Enabled = true;
        private IPauseService _pauseService;

        [Inject]
        private void Init(IGameFactory gameFactory, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _gameFactory = gameFactory;
            hero = _gameFactory.HeroDeathObject;
            _targetTransform = _gameFactory.HeroDeathObject.transform;

            hero.OnHeroDeath += HeroDie;
            _pauseService.PauseChanged += OnPauseChanged;
        }

        private void OnDisable() => 
            _pauseService.PauseChanged -= OnPauseChanged;

        private void OnPauseChanged()
        {
            if (_pauseService.IsPaused)
            {
                _rb.bodyType = RigidbodyType2D.Kinematic;
                _rb.linearVelocity = Vector2.zero;
            }
            else
                _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        private void HeroDie()
        {
           Enabled = false;
           animator.PlayOnHeroDie();
        }

        public void FixedUpdate()
        {
            if (_pauseService.IsPaused)
                return;
            float distance = Vector2.Distance(_rb.transform.position, _targetTransform.position);
            if (distance >= minimalDistance && Enabled) 
                Chase();
        }

        private void Chase()
        {
            Vector2 direction = (_targetTransform.position - _rb.transform.position).normalized;
            LookAtTarget(direction);
            Vector2 velocity = _rb.linearVelocity;
            velocity.x = speed * direction.x;
            _rb.linearVelocity = velocity;
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