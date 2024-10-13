using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private int offset = 3;
        
        private  Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            _camera.transform.position =  transform.position + new Vector3(0, offset, -10);
        }
    }
}
