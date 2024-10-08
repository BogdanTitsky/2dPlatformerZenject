﻿using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemyAnimator))]
    
    public class AnimateEnemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        
        private const float MinimalVelocity = 0.1f;

        private void Update()
        {
            if (ShouldMove())
                _enemyAnimator.Move();
            else
                _enemyAnimator.StopMoving();
        }

        private bool ShouldMove()
        {
            return Math.Abs(_rb.velocity.x) > MinimalVelocity;
        }
    }
}