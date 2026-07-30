using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle3D
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _light;
        [SerializeField] private RectTransform _button;
        [SerializeField] private ParticleSystem _confetti;

        private Tween _lightStartTween;
        private Tween _lightRotateTween;
        private Tween _buttonTween;

        public static WinScreen I { get; private set; }

        private void Awake()
        {
            I = this;

            _light.gameObject.SetActive(false);

            _button.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _lightStartTween.Kill();

            _lightRotateTween.Kill();

            _buttonTween.Kill();
        }

        public void LoadNextLevel()
        {
            AudioController.I.PlayButtonSound();

            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex + 1);
            }
        }

        [SerializeField] private float _delay = 1f;
        [SerializeField] private float _lightStartDuration = 0.2f;
        [SerializeField] private float _lightRotateDuration = 12f;
        [SerializeField] private float _buttonStartDuration = 1f;
        [SerializeField] private float _startScale = 0.9f;
        public void Show()
        {
            _lightStartTween = _light.DOScale(1, _lightStartDuration)
                .From(0)
                .SetEase(Ease.OutCubic)
                .SetDelay(_delay)
                .OnStart(() => _light.gameObject.SetActive(true));

            _lightRotateTween = _light.DORotate(new(0, 0, -180), _lightRotateDuration, RotateMode.FastBeyond360)
                .From(new Vector3(0, 0, 180))
                .SetEase(Ease.Linear)
                .SetDelay(_delay)
                .SetLoops(-1);

            _buttonTween = _button.DOScale(1, _buttonStartDuration)
                .From(_startScale)
                .SetEase(Ease.OutElastic)
                .SetDelay(_delay)
                .OnStart(() => _button.gameObject.SetActive(true));

            _confetti.Play();

            AudioController.I.PlayLevelCompleteSound();
        }
    }
}