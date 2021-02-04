using UnityEngine;

public class InventoryItemSlot : MonoBehaviour
{
    private Item.ItemType _itemType;
    private int _amount;

    public void SetAmount(int amount)
    {
        _amount = amount;
    }

    public int GetAmount()
    {
        return _amount;
    }


    public void SetItemType(Item.ItemType itemType)
    {
        _itemType = itemType;
    }

    public Item.ItemType GetItemType()
    {
        return _itemType;
    }

}
