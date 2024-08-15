using UnityEngine;

namespace CodeBase.CameraLogic._2D_Camera.Scripts
{
    public class InputHandler : MonoBehaviour {

        private void Awake() {
            // Handle cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}