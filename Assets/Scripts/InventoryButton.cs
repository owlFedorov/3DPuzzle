using UnityEngine;
using UnityEngine.UI;

namespace Puzzle3D
{
    public class InventoryButton : MonoBehaviour
    {
        public static InventoryButton ActiveButton { get; private set; }
        public Item Item { get; private set; }

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ButtonClickedHandler);
        }

        private void OnDestroy()
        {
            if (Item != null)
            {
                Item.Placed -= ResetButton;
            }
        }

        private void ButtonClickedHandler()
        {
            if (ActiveButton == this)
            {
                ActiveButton = null;
            }
            else
            {
                ActiveButton = this;
            }
        }

        public void SetItem(Item item)
        {
            Item = item;

            Item.Placed += ResetButton;

            gameObject.SetActive(true);

            transform.SetAsLastSibling();

            GetComponentInChildren<Text>().text = item.gameObject.name;
        }

        private void ResetButton(Item item)
        {
            ActiveButton = null;

            Item.Placed -= ResetButton;

            Item = null;

            gameObject.SetActive(false);

            Inventory.I.DeactivateButton(this);
        }
    }
}