using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraTarget : MonoBehaviour
    {
        private Camera _camera;
        private CameraStats _stats;

        private void Awake()
        {
            _camera = Camera.main;
            _stats = _camera.GetComponent<CameraStats>();
        }

        private void FixedUpdate()
        {
            Vector3 targetPosition = transform.position + new Vector3(0, _stats.Offset, -10);
            float distance = Vector3.Distance(_camera.transform.position, targetPosition);

            float speed = _stats.CameraSpeed + distance * _stats.MaxSpeedMultiplier;

            targetPosition.y = Mathf.Max(targetPosition.y, _stats.MinY);

            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize,  _stats.Size, Time.deltaTime * _stats.CameraZoomSpeed);
            _camera.transform.position =
                Vector3.Lerp(_camera.transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
