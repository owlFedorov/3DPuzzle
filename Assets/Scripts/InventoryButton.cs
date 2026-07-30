using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle3D
{
    public class InventoryButton : MonoBehaviour
    {
        public static event Action<InventoryButton> Clicked;

        [SerializeField] private Image _itemImage;

        private RectTransform rt;
        private Button _button;
        private Tween _scaleTween;
        private Tween _moveTween;
        private float _defaultPositionY;

        public static InventoryButton ActiveButton { get; private set; }
        public Item Item { get; private set; }

        private void Awake()
        {
            rt = GetComponent<RectTransform>();

            _defaultPositionY = rt.anchoredPosition.y;

            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(ButtonClickedHandler);

            _button.interactable = false;

            transform.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (Item != null)
            {
                Item.Placed -= ResetButton;
            }

            _scaleTween.Kill();

            _moveTween.Kill();

            ActiveButton = null;
        }

        public void SetItem(Item item)
        {
            Item = item;

            Item.Placed += ResetButton;

            _itemImage.sprite = Item.Sprite;

            PlayScaleUpAnimation();

            _button.interactable = true;
        }

        private void ButtonClickedHandler()
        {
            if (ActiveButton == this)
            {
                ActiveButton = null;

                PlayMoveDownAnimation();
            }
            else
            {
                if (ActiveButton != null)
                {
                    ActiveButton.PlayMoveDownAnimation();
                }

                ActiveButton = this;

                PlayMoveUpAnimation();
            }

            Clicked?.Invoke(ActiveButton);
        }

        private void ResetButton(Item item)
        {
            ActiveButton = null;

            Item.Placed -= ResetButton;

            Item = null;

            PlayMoveDownAnimation();

            PlayScaleDownAnimation();

            _button.interactable = false;
        }

        [SerializeField] private float _scaleSize = 0.8f;
        [SerializeField] private float _duration = 0.25f;
        private void PlayScaleUpAnimation()
        {
            _scaleTween.Kill();

            _scaleTween = transform.DOScale(_scaleSize, _duration).SetEase(Ease.InCubic);
        }

        private void PlayScaleDownAnimation()
        {
            _scaleTween.Kill();

            _scaleTween = transform.DOScale(0, _duration).SetEase(Ease.OutCubic).OnComplete(() => Inventory.I.DeactivateButton(this));
        }

        [SerializeField, Range(0, 1f)] private float _yOffset = 40f / 1080f;
        private void PlayMoveUpAnimation()
        {
            _moveTween.Kill();

            float width = (transform.parent as RectTransform).rect.width;

            _moveTween = rt.DOAnchorPosY(_defaultPositionY + width * _yOffset, _duration).SetEase(Ease.OutCubic);

            _scaleTween.Kill();

            _scaleTween = transform.DOScale(1, _duration).SetEase(Ease.OutCubic);

            AudioController.I.PlayItemSelectSound();
        }

        private void PlayMoveDownAnimation()
        {
            _moveTween.Kill();

            _moveTween = rt.DOAnchorPosY(_defaultPositionY, _duration).SetEase(Ease.OutCubic);

            _scaleTween.Kill();

            _scaleTween = transform.DOScale(_scaleSize, _duration).SetEase(Ease.OutCubic);
        }
    }
}