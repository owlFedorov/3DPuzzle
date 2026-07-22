using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle3D
{
    public class PlaceButton : MonoBehaviour
    {
        private RectTransform rt;
        private Sequence _sequence;

        public Place Place { get; set; }

        private void Awake()
        {
            rt = GetComponent<RectTransform>();

            GetComponent<Button>().onClick.AddListener(ButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        public void UpdatePositionAndSize(Vector2 position, float size)
        {
            rt.position = position;

            rt.sizeDelta = new Vector2(size, size);
        }

        [SerializeField] private float _delay = 1f;
        [SerializeField] private float _scaleDuration = 0.25f;
        public void PlayStartAnimation()
        {
            gameObject.SetActive(true);

            _sequence.Kill();

            _sequence = DOTween.Sequence();

            _sequence.AppendInterval(_delay)
                .Append(transform.DOScale(1, _scaleDuration).From(0).SetEase(Ease.InCubic));
        }

        private void ButtonClickedHandler()
        {
            if (PlaceManager.I.CheckItem(this) == false)
            {
                PlayErrorAnimation();
            }
        }

        [SerializeField] private Vector2 _punch = new(20, 0);
        [SerializeField] private float _punchDuration = 0.5f;
        private void PlayErrorAnimation()
        {
            _sequence.Kill();

            _sequence = DOTween.Sequence();

            _sequence.Append(rt.DOPunchAnchorPos(_punch, _punchDuration));
        }
    }
}