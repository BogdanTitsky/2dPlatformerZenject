using System;
using UnityEngine;

namespace CodeBase.Hero
{
    public class GroundChecker : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        [SerializeField] private LayerMask _layerGround;
        private readonly Collider2D[] groundColliders = new Collider2D[1];
        private Vector2 startPoint;
        private Vector2 size = new(1f, 0.1f);

        private void Update()
        {
            startPoint = new Vector2(transform.position.x, transform.position.y);
            
            var numColliders = Physics2D.OverlapCapsuleNonAlloc(startPoint,
                size, CapsuleDirection2D.Horizontal, 0, groundColliders, _layerGround);

            IsGrounded = numColliders > 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(startPoint, size);
        }
    }
}