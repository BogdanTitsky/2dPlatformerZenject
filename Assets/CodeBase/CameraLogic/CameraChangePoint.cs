using System;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraChangePoint : MonoBehaviour
    {
        public float NewOffset = 4;
        public float NewSize = 6.5f;
        private CameraStats _cameraStats;


        private void Awake()
        {
            _cameraStats = Camera.main.GetComponent<CameraStats>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            _cameraStats.Offset = NewOffset;
            _cameraStats.Size = NewSize;
        }
        
        private void OnTriggerExit2D(Collider2D other) => _cameraStats.ResetStats();
    }
}
