using System;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Repair,
        Void,
        Slowmo
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetInventorySprite()
    {
        switch (itemType)
        {
            case ItemType.Repair: return ItemAssets.Instance.repairInventoryImage;
            case ItemType.Void: return ItemAssets.Instance.voidInventoryImage;
            case ItemType.Slowmo: return ItemAssets.Instance.slowmoInventoryImage;
        }
        Debug.LogError("Failed to get item's sprite!");
        return null;
    }

    public Color GetColor()
    {
        switch (itemType)
        {
            case ItemType.Repair: return ItemAssets.Instance.repairItemColor;
            case ItemType.Void: return ItemAssets.Instance.voidItemColor;
            case ItemType.Slowmo: return ItemAssets.Instance.slowmoItemColor;
        }
        Debug.LogError("Failed to get item's sprite!");
        return Color.magenta;
    }

    public Sprite GetIcon()
    {
        switch (itemType)
        {
            case ItemType.Repair: return ItemAssets.Instance.repairItemWorldImage;
            case ItemType.Void: return ItemAssets.Instance.voidItemWorldImage;
            case ItemType.Slowmo: return ItemAssets.Instance.slowmoItemWorldImage;
        }
        Debug.LogError("Failed to get item's sprite!");
        return null;
    }

    public Sprite GetWorldSprite()
    {
        switch (itemType)
        {
            case ItemType.Repair: return ItemAssets.Instance.repairItemWorldImage;
            case ItemType.Void: return ItemAssets.Instance.voidItemWorldImage;
            case ItemType.Slowmo: return ItemAssets.Instance.slowmoItemWorldImage;
        }
        Debug.LogError("Failed to get item's sprite!");
        return null;
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
                return false;
            case ItemType.Repair:
            case ItemType.Void:
            case ItemType.Slowmo:
                return true;
        }
    }
}

