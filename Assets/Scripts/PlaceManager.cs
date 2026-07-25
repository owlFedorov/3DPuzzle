using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle3D
{
    public class PlaceManager : MonoBehaviour
    {
        public static PlaceManager I { get; private set; }

        [SerializeField] private GameObject _placeButtonPrefab;
        [SerializeField] private RectTransform _placeButtonsParent;
        [SerializeField, Range(0, 1f)] private float _buttonSizeRatio = 0.1f;

        private Camera _camera;
        private readonly List<Place> _places = new();
        private PlaceButton[] _buttons;
        private int[] _buttonRequestsCounters;

        private void Awake()
        {
            I = this;

            _camera = Camera.main;
        }

        private void Start()
        {
            _places.AddRange(GetComponentsInChildren<Place>(true));

            _buttons = new PlaceButton[_places.Count];

            _buttonRequestsCounters = new int[_places.Count];

            for (int i = 0; i < _buttons.Length; i++)
            {
                GameObject go = Instantiate(_placeButtonPrefab, _placeButtonsParent);

                RectTransform rt = go.GetComponent<RectTransform>();

                rt.anchoredPosition = _camera.WorldToScreenPoint(_places[i].transform.position);

                _buttons[i] = go.GetComponent<PlaceButton>();

                _buttons[i].Place = _places[i];

                _buttons[i].gameObject.SetActive(false);
            }
        }

        private float _lastWidth;
        private bool _isNextFrame;
        private void Update()
        {
            float width = _placeButtonsParent.rect.width;

            if (width != _lastWidth)
            {
                if (_isNextFrame == false)
                {
                    _isNextFrame = true;
                }
                else
                {
                    _isNextFrame = false;

                    _lastWidth = width;

                    float size = width * _buttonSizeRatio;

                    for (int i = 0; i < _buttons.Length; i++)
                    {
                        Vector3 position = _camera.WorldToScreenPoint(_places[i].transform.position);

                        _buttons[i].UpdatePositionAndSize(position, size);
                    }
                }
            }
        }

        public bool CheckItem(PlaceButton button)
        {
            if (InventoryButton.ActiveButton != null)
            {
                if (InventoryButton.ActiveButton.Item.CheckPlace(button.Place) == true)
                {
                    HideButton(button);

                    AudioController.I.PlayPlaceCorrectSound();

                    return true;
                }
            }

            AudioController.I.PlayPlaceWrongSound();

            return false;
        }

        public void ShowButton(Place place)
        {
            int index = _places.IndexOf(place);

            if (index == -1) return;

            _buttonRequestsCounters[index]++;

            _buttons[index].PlayStartAnimation();
        }

        private void HideButton(PlaceButton button)
        {
            int index = Array.IndexOf(_buttons, button);

            if (index == -1) return;

            _buttonRequestsCounters[index]--;

            if (_buttonRequestsCounters[index] > 0)
            {
                _buttons[index].PlayStartAnimation();
            }
            else
            {
                _buttons[index].gameObject.SetActive(false);
            }
        }
    }
}