using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private int offset = 4;
        [SerializeField] private float cameraSpeed = 5;
        [SerializeField] private float maxSpeedMultiplier = 2f;
        [SerializeField] private float minY = 1f;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            Vector3 targetPosition = transform.position + new Vector3(0, offset, -10);
            float distance = Vector3.Distance(_camera.transform.position, targetPosition);

            float speed = cameraSpeed + distance * maxSpeedMultiplier;

            targetPosition.y = Mathf.Max(targetPosition.y, minY);

            _camera.transform.position =
                Vector3.Lerp(_camera.transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
