using DG.Tweening;
using UnityEngine;

namespace Puzzle3D
{
    public enum SpawnType
    {
        Move,
        Scale,
    }

    public class ItemAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _view;
        [SerializeField] private SpawnType _spawnType;
        [SerializeField] private float _moveDuration = 0.5f;
        [SerializeField] private Vector3 _startPositionOffset;
        [SerializeField] private float _shakeDuration = 0.75f;
        [SerializeField] private Vector3 _strength = new(0.25f, 0.25f, 0.25f);
        [SerializeField] private int _vibrato = 7;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Harmonic;
        [SerializeField] private float _scaleDuration = 1f;

        private Sequence _sequence;

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        public void Spawn()
        {
            if (_spawnType == SpawnType.Move)
            {
                Vector3 endPosition = _view.position;

                _view.localPosition += _startPositionOffset;

                _sequence = DOTween.Sequence();

                _sequence.Append(_view.DOMove(endPosition, _moveDuration).SetEase(Ease.InSine))
                    .AppendCallback(PlayParticlesAnimation)
                    .Append(_view.DOShakeScale(_shakeDuration, _strength, _vibrato, _randomness, _fadeOut, _randomnessMode));
            }
            else
            {
                _sequence = DOTween.Sequence();

                _sequence.AppendCallback(PlayParticlesAnimation)
                    .Append(_view.DOScale(Vector3.one, _scaleDuration).From(Vector3.zero).SetEase(Ease.OutElastic));
            }
        }

        private void PlayParticlesAnimation()
        {
            GetComponent<ParticleSystem>().Play();
        }
    }
}