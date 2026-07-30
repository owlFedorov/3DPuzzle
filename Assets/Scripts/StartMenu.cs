using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle3D
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform _light;
        [SerializeField] private float _lightRotateDuration = 12;

        private Tween _tween;

        private void Start()
        {
            _tween = _light.DORotate(new(0, 0, -180), _lightRotateDuration, RotateMode.FastBeyond360)
                .From(new Vector3(0, 0, 180))
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }

        public void StartGame()
        {
            AudioController.I.PlayButtonSound();

            SceneManager.LoadScene(1);
        }
    }
}