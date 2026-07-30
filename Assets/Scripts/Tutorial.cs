using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace YG
{
    public partial class SavesYG
    {
        public bool TutorialIsComplete;
    }
}

namespace Puzzle3D
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private RectTransform _preview;
        [SerializeField] private RectTransform _previewSmall;
        [SerializeField] private RectTransform _inventory;
        [SerializeField] private RectTransform _place;
        [SerializeField] private RectTransform _placeButtonsParent;

        private Button _placeButton;
        private RectTransform _currentRT;
        private Vector2 _startPosition;
        private Tween _tween;
        private bool _inventoryButtonIsClicked;

        private void Awake()
        {
            if (YG2.saves.TutorialIsComplete == true)
            {
                gameObject.SetActive(false);

                return;
            }

            InventoryButton.Clicked += InventoryButtonClickedHandler;
        }

        private void Start()
        {
            if (YG2.saves.TutorialIsComplete == true) return;

            Preview.I.StateChanged += PreviewStateChangedHandler;

            PlayForefingerAnimation(_preview);
        }

        private void OnDestroy()
        {
            _tween.Kill();

            if (_currentRT != null)
            {
                _currentRT.gameObject.SetActive(false);
            }

            Preview.I.StateChanged -= PreviewStateChangedHandler;
            Preview.I.StateChanged -= PreviewStateChangedHandlerSecond;

            InventoryButton.Clicked -= InventoryButtonClickedHandler;

            if (_placeButton != null)
            {
                _placeButton.onClick.RemoveListener(PlaceButtonClickedHandler);
            }
        }

        private void SubscribeToButton()
        {
            foreach (Button button in _placeButtonsParent.GetComponentsInChildren<Button>())
            {
                if (button.gameObject.activeSelf == true)
                {
                    _placeButton = button;

                    break;
                }
            }

            _placeButton.onClick.AddListener(PlaceButtonClickedHandler);
        }

        private void PreviewStateChangedHandler(bool isOpen)
        {
            if (isOpen == true)
            {
                PlayForefingerAnimation(_preview);
            }
            else
            {
                if (_placeButton == null)
                {
                    SubscribeToButton();
                }

                InventoryButtonClickedHandler(InventoryButton.ActiveButton);
            }
        }
        private void PreviewStateChangedHandlerSecond(bool isOpen)
        {
            OnDestroy();

            gameObject.SetActive(false);

            YG2.saves.TutorialIsComplete = true;

            YG2.SaveProgress();
        }

        private void InventoryButtonClickedHandler(InventoryButton activeButton)
        {
            if (activeButton == null)
            {
                PlayForefingerAnimation(_inventory);

                _inventoryButtonIsClicked = false;
            }
            else
            {
                PlayForefingerAnimation(_place);

                _inventoryButtonIsClicked = true;
            }
        }

        private void PlaceButtonClickedHandler()
        {
            if (_inventoryButtonIsClicked == true)
            {
                OnDestroy();

                Preview.I.StateChanged += PreviewStateChangedHandlerSecond;

                PlayForefingerAnimation(_previewSmall);
            }
        }

        [SerializeField, Range(0, 1f)] private float _yOffset = 40f / 1080f;
        [SerializeField] private float _duration = 0.5f;
        private void PlayForefingerAnimation(RectTransform rt)
        {
            _tween.Kill();

            if (_currentRT != null)
            {
                _currentRT.gameObject.SetActive(false);

                _currentRT.anchoredPosition = _startPosition;
            }

            _currentRT = rt;

            _currentRT.gameObject.SetActive(true);

            _startPosition = _currentRT.anchoredPosition;

            float width = (rt.parent as RectTransform).rect.width;

            _tween = _currentRT.DOAnchorPosY(_startPosition.y + width * _yOffset, _duration).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Yoyo);
        }
    }
}