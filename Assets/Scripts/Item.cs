using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle3D
{
    [RequireComponent(typeof(ItemAnimator))]
    public class Item : MonoBehaviour
    {
        public event Action<Item> Placed;

        [SerializeField] private List<Item> _requiredItems;
        [SerializeField] private Sprite _sprite;

        private Place _place;

        public Sprite Sprite => _sprite;

        private void Awake()
        {
            _place = transform.parent.GetComponent<Place>();
        }

        private void Start()
        {
            SubscribeToRequiredItems();

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UnsubscribeToRequiredItems();
        }

        public bool CheckPlace(Place place)
        {
            if (place != _place) return false;

            gameObject.SetActive(true);

            GetComponent<ItemAnimator>().Spawn();

            Placed?.Invoke(this);

            return true;
        }

        private void SubscribeToRequiredItems()
        {
            if (_requiredItems == null || _requiredItems.Count == 0)
            {
                OnAllRequiredItemsPlaced();

                return;
            }

            foreach (var item in _requiredItems)
            {
                item.Placed += RequiredItemPlacedHandler;
            }
        }

        private void UnsubscribeToRequiredItems()
        {
            if (_requiredItems == null || _requiredItems.Count == 0) return;

            foreach (var item in _requiredItems)
            {
                item.Placed -= RequiredItemPlacedHandler;
            }
        }

        private void RequiredItemPlacedHandler(Item item)
        {
            if (_requiredItems.Contains(item) == false) return;

            item.Placed -= RequiredItemPlacedHandler;

            _requiredItems.Remove(item);

            if (_requiredItems.Count == 0)
            {
                OnAllRequiredItemsPlaced();
            }
        }

        private void OnAllRequiredItemsPlaced()
        {
            Inventory.I.AddItem(this);

            PlaceManager.I.ShowButton(_place);
        }
    }
}