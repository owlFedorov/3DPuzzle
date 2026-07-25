using UnityEngine;

namespace Puzzle3D
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector2 _referenceResolution = new(1080f, 2000f);

        private Camera _camera;
        private float _defaultCameraSize;
        private float _defaultAspect;
        private float _lastSize;

        public static CameraController I { get; private set; }

        private void Awake()
        {
            I = this;

            _camera = GetComponent<Camera>();

            _defaultCameraSize = _camera.orthographicSize;

            _defaultAspect = _referenceResolution.x / _referenceResolution.y;
        }

        private void LateUpdate()
        {
            UpdateCameraSize();
        }

        private void UpdateCameraSize()
        {
            float cameraAspect = _camera.aspect;

            if (cameraAspect >= _defaultAspect)
            {
                if (_camera.orthographicSize != _defaultCameraSize)
                {
                    _camera.orthographicSize = _defaultCameraSize;
                }
            }
            else
            {
                float size = _defaultCameraSize * _defaultAspect / cameraAspect;

                if (size != _lastSize)
                {
                    _lastSize = size;

                    _camera.orthographicSize = size;
                }
            }
        }
    }
}