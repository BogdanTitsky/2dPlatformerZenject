using UnityEngine;

namespace CodeBase.Hero
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerGround; 
        [SerializeField] private float checkDistance = 0.1f;
        [SerializeField] private Vector2 boxSize = new(1f, 0.1f);
        public bool IsGrounded { get; private set; }

        private void Update() => CheckIfGrounded();

        private void CheckIfGrounded()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, 
                boxSize, 0f, Vector2.down, checkDistance, _layerGround);
            IsGrounded = hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Vector3 position = transform.position;
            Gizmos.DrawWireCube(position * Vector2.down * checkDistance, boxSize);
        }
    }
}