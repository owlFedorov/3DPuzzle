using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle3D
{
    public class PlaceManager : MonoBehaviour
    {
        public static PlaceManager I { get; private set; }

        [SerializeField] private GameObject _placeButtonPrefab;
        [SerializeField] private Canvas _placeButtonsCanvas;

        private readonly List<Place> _places = new();
        private PlaceButton[] _buttons;
        private int[] _buttonRequestsCounters;

        private void Awake()
        {
            I = this;

            _places.AddRange(GetComponentsInChildren<Place>(true));

            _buttons = new PlaceButton[_places.Count];

            _buttonRequestsCounters = new int[_places.Count];

            Camera camera = Camera.main;

            for (int i = 0; i < _buttons.Length; i++)
            {
                GameObject go = Instantiate(_placeButtonPrefab, _placeButtonsCanvas.transform);

                RectTransform rt = go.GetComponent<RectTransform>();

                rt.anchoredPosition = camera.WorldToScreenPoint(_places[i].transform.position);

                _buttons[i] = go.GetComponent<PlaceButton>();

                _buttons[i].Place = _places[i];

                _buttons[i].gameObject.SetActive(false);
            }
        }

        public void ButtonClickedHandler(PlaceButton button)
        {
            if (InventoryButton.ActiveButton != null)
            {
                if (InventoryButton.ActiveButton.Item.CheckPlace(button.Place) == true)
                {
                    HideButton(button);
                }
            }
        }

        public void ShowButton(Place place)
        {
            int index = _places.IndexOf(place);

            if (index == -1) return;

            _buttonRequestsCounters[index]++;

            _buttons[index].gameObject.SetActive(true);
        }

        private void HideButton(PlaceButton button)
        {
            int index = Array.IndexOf(_buttons, button);

            if (index == -1) return;

            _buttonRequestsCounters[index]--;

            if (_buttonRequestsCounters[index] <= 0)
            {
                _buttons[index].gameObject.SetActive(false);
            }
        }
    }
}