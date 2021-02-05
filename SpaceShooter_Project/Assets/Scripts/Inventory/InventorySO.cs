using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Game/Inventory/Inventory")]
public class InventorySO : ScriptableObject
{
    public event EventHandler OnItemListChanged;

    public event EventHandler<Item> OnItemListAdded;

    public event EventHandler<Item> OnItemListRemoved;

    private List<Item> _itemList;

    private void OnEnable()
    {
        _itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in _itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }

            if (!itemAlreadyInInventory)
            {
                _itemList.Add(item);
            }
        }
        else
        {
            _itemList.Add(item);
        }
        OnItemListAdded?.Invoke(this, item);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in _itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                _itemList.Remove(itemInInventory);
            }
        }
        else
        {
            _itemList.Remove(item);
        }
        OnItemListRemoved?.Invoke(this, item);

    }

    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Repair:
                PlayerHealthShield playerHealthShield = FindObjectOfType<PlayerHealthShield>();

                if (playerHealthShield != null)
                {
                    RemoveItem(new Item { itemType = Item.ItemType.Repair, amount = 1 });
                    playerHealthShield.Repair();
                    AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.Repair);
                }
                break;
            case Item.ItemType.Void:
                Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
                RemoveItem(new Item { itemType = Item.ItemType.Void, amount = 1 });
                Instantiate(GameAssets.Instance.voidEffectPrefab, playerTransform.position, Quaternion.identity);
                AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ItemUseDefaultSfx);
                break;
            case Item.ItemType.Slowmo:
                TimeManager.Instance.DoSlowmotion();
                RemoveItem(new Item { itemType = Item.ItemType.Slowmo, amount = 1 });
                AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ItemUseDefaultSfx);
                break;
        }
    }

    public List<Item> GetItemList()
    {
        return _itemList;
    }
}
