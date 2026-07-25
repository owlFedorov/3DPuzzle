using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle3D
{
    public class Preview : MonoBehaviour
    {
        public event Action<bool> StateChanged;

        [SerializeField] private bool _isOpen;
        [SerializeField] private RectTransform _panelRT;
        [SerializeField] private float _scaleDuration = 0.25f;
        [SerializeField] private float _openScale = 5f;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private float _openFade = 0.5f;

        private Tween _scaleTween;
        private Tween _fadeTween;

        public static Preview I {  get; private set; }

        private void Awake()
        {
            I = this;

            Color color = _backgroundImage.color;

            if (_isOpen == true)
            {
                _panelRT.localScale = new(_openScale, _openScale, _openScale);

                color.a = _openFade;

                _backgroundImage.color = color;
            }
            else
            {
                _panelRT.localScale = Vector3.one;

                color.a = 0;

                _backgroundImage.color = color;
            }

            _backgroundImage.gameObject.SetActive(_isOpen);
        }

        private void OnDestroy()
        {
            _scaleTween.Kill();

            _fadeTween.Kill();
        }

        public void OpenClosePreview()
        {
            if (_isOpen == true)
            {
                _scaleTween.Kill();

                _scaleTween = _panelRT.DOScale(Vector3.one, _scaleDuration).SetEase(Ease.OutCubic);

                _fadeTween.Kill();

                _fadeTween = _backgroundImage.DOFade(0, _scaleDuration).SetEase(Ease.OutCubic)
                    .OnComplete(() => _backgroundImage.gameObject.SetActive(false));
            }
            else
            {
                _scaleTween.Kill();

                _scaleTween = _panelRT.DOScale(_openScale, _scaleDuration).SetEase(Ease.InCubic);

                _backgroundImage.gameObject.SetActive(true);

                _fadeTween.Kill();

                _fadeTween = _backgroundImage.DOFade(_openFade, _scaleDuration).SetEase(Ease.InCubic);
            }

            _isOpen = !_isOpen;

            AudioController.I.PlayPreviewSound();

            StateChanged?.Invoke(_isOpen);
        }
    }
}