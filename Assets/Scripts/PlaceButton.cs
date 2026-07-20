using UnityEngine;
using UnityEngine.UI;

namespace Puzzle3D
{
    public class PlaceButton : MonoBehaviour
    {
        private RectTransform rt;

        public Place Place { get; set; }

        private void Awake()
        {
            rt = GetComponent<RectTransform>();

            GetComponent<Button>().onClick.AddListener(ButtonClickedHandler);
        }

        public void UpdatePositionAndSize(Vector2 position, float size)
        {
            rt.anchoredPosition = position;

            rt.sizeDelta = new Vector2(size, size);
        }

        private void ButtonClickedHandler()
        {
            PlaceManager.I.ButtonClickedHandler(this);
        }
    }
}