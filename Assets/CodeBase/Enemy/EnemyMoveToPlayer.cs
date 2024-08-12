using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;

        private Transform targetTransform;

        public void FixedUpdate()
        {
            
        }
    }
}