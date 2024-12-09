using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 1f), Tooltip("The higher the value, the faster the background moves relative to the camera. A value of 1 means the background moves at the same speed as the camera.")]
        private float parallaxX = 1f;
        [SerializeField, Range(0.01f, 1f), Tooltip("The higher the value, the faster the background moves relative to the camera. A value of 1 means the background moves at the same speed as the camera.")]
        private float parallaxY = 1f;
        [SerializeField] private SpriteRenderer sprite;
        
        private Transform _cameraTransform;
        private Vector3 _previousCameraPosition;
        private float _textureUnitSizeX;
        
        private void Start()
        {
         _cameraTransform = Camera.main.transform;
         _previousCameraPosition = _cameraTransform.position;
         Texture2D texture = sprite.sprite.texture;
         _textureUnitSizeX = texture.width / sprite.sprite.pixelsPerUnit;
        }

        private void LateUpdate()
        {
                Vector3 deltaMovement = _cameraTransform.position - _previousCameraPosition;
                transform.position += new Vector3(deltaMovement.x * parallaxX, deltaMovement.y) ;
                _previousCameraPosition = _cameraTransform.position;
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
            {
                float offset = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offset, transform.position.y);
            }
        }
    }
}
