using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Vector2 parallaxEffectMultiplier;
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
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y) ;
            _previousCameraPosition = _cameraTransform.position;

            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
            {
                float offset = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offset, transform.position.y);
            }
        }
    }
}
