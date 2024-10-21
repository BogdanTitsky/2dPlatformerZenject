using UnityEngine;

namespace CodeBase.Hero
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerGround; 
        [SerializeField] private float checkDistance = 0.1f;
        public bool IsGrounded { get; private set; }

        private void Update() => CheckIfGrounded();

        private void CheckIfGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, 
                Vector2.down, checkDistance, _layerGround);
            IsGrounded = hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Vector3 position = transform.position;
            Gizmos.DrawLine(position, position + Vector3.down * checkDistance);
        }
    }
}