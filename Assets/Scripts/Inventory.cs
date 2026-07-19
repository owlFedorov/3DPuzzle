using System.Collections.Generic;
using UnityEngine;

namespace Puzzle3D
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory I { get; private set; }

        private readonly List<InventoryButton> _inactiveButtons = new();
        private readonly List<InventoryButton> _activeButtons = new();
        private readonly List<Item> _items = new();

        private void Awake()
        {
            I = this;

            InventoryButton[] buttons = GetComponentsInChildren<InventoryButton>(true);

            foreach (InventoryButton button in buttons)
            {
                _inactiveButtons.Add(button);

                button.gameObject.SetActive(false);
            }
        }

        public void AddItem(Item item)
        {
            if (_items.Contains(item) == true) return;

            _items.Add(item);

            ActivateButton();
        }

        public void DeactivateButton(InventoryButton button)
        {
            if (_activeButtons.Contains(button) == false) return;

            _activeButtons.Remove(button);

            _inactiveButtons.Add(button);

            ActivateButton();
        }

        private void ActivateButton()
        {
            if (_inactiveButtons.Count == 0 || _items.Count == 0) return;

            InventoryButton button = _inactiveButtons[0];

            _inactiveButtons.Remove(button);

            _activeButtons.Add(button);

            Item item = _items[Random.Range(0, _items.Count)];

            _items.Remove(item);

            button.SetItem(item);
        }
    }
}