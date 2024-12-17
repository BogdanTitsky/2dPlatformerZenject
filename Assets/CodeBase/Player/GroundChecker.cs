using System;
using UnityEngine;

namespace CodeBase.Player
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Collider2D colliderChecker;
        
        public bool IsGrounded
        {
            get => isGrounded;
            private set
            {
                if (isGrounded == value)
                    return;
                isGrounded = value;
                GroundedChanged?.Invoke();
            }
        }

        private bool isGrounded;
        public event Action GroundedChanged;

        private void OnTriggerStay2D(Collider2D other) => IsGrounded = Mathf.Abs(rb.linearVelocity.y) <= 0.1f;

        private void OnTriggerExit2D(Collider2D other) => IsGrounded = false;
    }
}