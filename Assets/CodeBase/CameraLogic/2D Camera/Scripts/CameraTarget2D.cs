using UnityEngine;

namespace CodeBase.CameraLogic._2D_Camera.Scripts
{
    public class CameraTarget2D : MonoBehaviour
    {

        private void Awake() {
        
        }

        /// <summary>
        /// Draw gizmos.
        /// </summary>
        private void OnDrawGizmos() {
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
