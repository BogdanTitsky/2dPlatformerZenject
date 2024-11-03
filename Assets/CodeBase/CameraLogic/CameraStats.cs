using System;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraStats : MonoBehaviour
    {
        [SerializeField] private Camera camera;
         
        public float Offset = 0;
        public float CameraSpeed = 5;
        public float CameraZoomSpeed = 2;
        public float MaxSpeedMultiplier = 2;
        public float MinY = 0;
        public float Size;

        private float StartOffset;
        private float StartSize;

        private void Awake()
        {
            StartOffset = Offset;
            StartSize = Size;
        }

        public void ResetStats()
        {
            Offset = StartOffset;
            Size = StartSize;
        }
    }
}