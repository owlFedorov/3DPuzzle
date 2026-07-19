using UnityEngine;
using UnityEngine.UI;

namespace Puzzle3D
{
    public class PlaceButton : MonoBehaviour
    {
        public Place Place { get; set; }

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ButtonClickedHandler);
        }

        private void ButtonClickedHandler()
        {
            PlaceManager.I.ButtonClickedHandler(this);
        }
    }
}