using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle3D
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _topPanel;
        [SerializeField] private RectTransform _button;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _topPanelOffset = 780f;
        [SerializeField] private float _buttonOffset = 820f;

        private Sequence _sequence;

        public static WinScreen I { get; private set; }

        private void Awake()
        {
            I = this;

            Vector2 position = _topPanel.anchoredPosition;

            position.y += _topPanelOffset;

            _topPanel.anchoredPosition = position;

            position = _button.anchoredPosition;

            position.y -= _buttonOffset;

            _button. anchoredPosition = position;
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        public void LoadNextLevel()
        {
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

        public void Show()
        {
            _sequence = DOTween.Sequence();

            _sequence.AppendInterval(_delay)
                .Append(_topPanel.DOAnchorPosY(_topPanel.anchoredPosition.y - _topPanelOffset, _duration).SetEase(Ease.OutCubic))
                .Insert(_delay, _button.DOAnchorPosY(_button.anchoredPosition.y + _buttonOffset, _duration).SetEase(Ease.OutCubic));
        }
    }
}