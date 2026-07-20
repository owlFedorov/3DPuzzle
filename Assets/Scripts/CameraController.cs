using UnityEngine;

namespace Puzzle3D
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector2 _referenceResolution = new(1080f, 1920f);

        private Camera _camera;
        private float _defaultCameraSize;
        private float _defaultAspect;
        private float _defaultPositionY;
        private float _currentCameraAspect;
        private float _lastCameraAspect;

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            _defaultCameraSize = _camera.orthographicSize;

            _defaultAspect = _referenceResolution.x / _referenceResolution.y;

            _defaultPositionY = transform.position.y;

            LateUpdate();
        }

        private void LateUpdate()
        {
            _currentCameraAspect = _camera.aspect;

            if (_lastCameraAspect != _currentCameraAspect)
            {
                UpdateCameraSize();
            }
        }

        private void UpdateCameraSize()
        {
            if (_currentCameraAspect >= _defaultAspect)
            {
                if (_camera.orthographicSize != _defaultCameraSize)
                {
                    _camera.orthographicSize = _defaultCameraSize;
                }

                if (transform.position.y != _defaultPositionY)
                {
                    transform.position = new(transform.position.x, _defaultPositionY, transform.position.z);
                }
            }
            else
            {
                float size = _defaultCameraSize / _currentCameraAspect * _defaultAspect;

                _camera.orthographicSize = size;

                Vector3 newPosition = transform.position;

                newPosition.y = _defaultPositionY / _defaultCameraSize * size;

                transform.position = newPosition;
            }

            _lastCameraAspect = _currentCameraAspect;
        }
    }
}