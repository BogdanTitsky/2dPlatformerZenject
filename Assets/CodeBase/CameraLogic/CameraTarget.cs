using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private int Offset = 3;
        [SerializeField] private float CameraSpeed = 5;
        
        
        private  Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            Vector3 newPosition = transform.position + new Vector3(0, Offset, -10);
            _camera.transform.position =  Vector3.Slerp(_camera.transform.position, newPosition, CameraSpeed * Time.deltaTime);
        }
    }
}
