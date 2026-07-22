using System;
using UnityEngine;

namespace Puzzle3D
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public event Action ScreenUpdated;

        [SerializeField] private Vector2 _referenceResolution = new(1080f, 1920f);
        [SerializeField] private RectTransform _target;
        [SerializeField] private Transform _level;

        private Camera _camera;
        private Vector3 _levelOffset;
        private float _defaultCameraSize;
        private float _defaultAspect;
        private float _lastTargetPositionY;

        public static CameraController I { get; private set; }

        private void Awake()
        {
            I = this;

            _camera = GetComponent<Camera>();

            _defaultCameraSize = _camera.orthographicSize;

            _defaultAspect = _referenceResolution.x / _referenceResolution.y;

            _levelOffset = _level.position - transform.position;
        }

        private void LateUpdate()
        {
            if (_target.position.y != _lastTargetPositionY)
            {
                _lastTargetPositionY = _target.position.y;

                UpdateCameraSize();

                ScreenUpdated?.Invoke();
            }
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

                if (_level.position != transform.position + _levelOffset)
                {
                    _level.position = transform.position + _levelOffset;
                }
            }
            else
            {
                _camera.orthographicSize = _defaultCameraSize * _defaultAspect / cameraAspect;

                _level.position = _target.position + _levelOffset;
            }
        }
    }
}