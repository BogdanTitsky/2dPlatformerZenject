using System;
using UnityEngine;

namespace CodeBase.Hero
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerGround; 
        [SerializeField] private float checkDistance = 0.1f;
        [SerializeField] private Vector2 boxSize = new(1f, 0.1f);
        [SerializeField] private Rigidbody2D rb;

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

        private void Update() => CheckIfGrounded();

        private void CheckIfGrounded()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, 
                boxSize, 0f, Vector2.down, checkDistance, _layerGround);
            IsGrounded = hit.collider != null && rb.linearVelocity.y <= 0.1f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Vector3 position = transform.position;
            Gizmos.DrawWireCube(position * Vector2.down * checkDistance, boxSize);
        }
    }
}