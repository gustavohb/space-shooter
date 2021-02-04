using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : ExtendedCustomMonoBehavior
{
    [SerializeField] private InventorySO _inventory = default;
    private Transform _itemSlotContainer;
    private Transform _itemSlotTemplate;

    private void Awake()
    {
        _itemSlotContainer = transform.Find("ItemSlotContainer");
        _itemSlotTemplate = _itemSlotContainer.Find("ItemSlotTemplate");
    }

    private void Start()
    {
        _inventory.OnItemListChanged += Inventory_OnItemListChanged;
        _inventory.OnItemListAdded += Inventory_OnItemListAdded;
        _inventory.OnItemListRemoved += Inventory_OnItemListRemoved;
    }

    private void Inventory_OnItemListRemoved(object sender, Item itemRemoved)
    {
        InventoryItemSlot inventoryItemSlot = GetItemTypeAlreadyInInventory(itemRemoved.itemType);
        RectTransform itemSlotRectTransform;

        if (inventoryItemSlot != null)
        {
            inventoryItemSlot.SetAmount(inventoryItemSlot.GetAmount() - itemRemoved.amount);

            itemSlotRectTransform = inventoryItemSlot.GetComponent<RectTransform>();

            Transform amountTransform = itemSlotRectTransform.Find("Amount");

            TextMeshProUGUI uiText = amountTransform.Find("Text").GetComponent<TextMeshProUGUI>();

            if (inventoryItemSlot.GetAmount() == 0)
            {
                Destroy(inventoryItemSlot.gameObject);
            }
            else if (inventoryItemSlot.GetAmount() > 1)
            {
                uiText.SetText(inventoryItemSlot.GetAmount().ToString());
                amountTransform.gameObject.SetActive(true);
            }
            else
            {
                amountTransform.gameObject.SetActive(false);
            }
        }
    }

    private void Inventory_OnItemListAdded(object sender, Item itemAdded)
    {
        InventoryItemSlot inventoryItemSlot = GetItemTypeAlreadyInInventory(itemAdded.itemType);
        RectTransform itemSlotRectTransform;

        if (inventoryItemSlot != null)
        {
            itemSlotRectTransform = inventoryItemSlot.GetComponent<RectTransform>();

            Transform amountTransform = itemSlotRectTransform.Find("Amount");

            TextMeshProUGUI uiText = amountTransform.Find("Text").GetComponent<TextMeshProUGUI>();

            inventoryItemSlot.SetAmount(inventoryItemSlot.GetAmount() + itemAdded.amount);

            uiText.SetText(inventoryItemSlot.GetAmount().ToString());
            if (inventoryItemSlot.GetAmount() > 1)
            {
                amountTransform.gameObject.SetActive(true);
            }
            else
            {
                amountTransform.gameObject.SetActive(false);
            }


        }
        else
        {
            itemSlotRectTransform = Instantiate(_itemSlotTemplate, _itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(() => {
                // Use item
                _inventory.UseItem(itemAdded);

            });

            itemSlotRectTransform.GetComponent<InventoryItemSlot>().SetItemType(itemAdded.itemType);
            itemSlotRectTransform.GetComponent<InventoryItemSlot>().SetAmount(itemAdded.amount);

            Image imagem = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            imagem.sprite = itemAdded.GetInventorySprite();

            Transform amountTransform = itemSlotRectTransform.Find("Amount");
            if (itemAdded.amount > 1)
            {
                TextMeshProUGUI uiText = amountTransform.Find("Text").GetComponent<TextMeshProUGUI>();
                uiText.SetText(itemAdded.amount.ToString());
            }
            else
            {
                amountTransform.gameObject.SetActive(false);
            }
        }



    }

    private InventoryItemSlot GetItemTypeAlreadyInInventory(Item.ItemType itemType)
    {
        foreach (Transform child in _itemSlotContainer)
        {
            if (child == _itemSlotTemplate) continue;
            if (child.GetComponent<InventoryItemSlot>().GetItemType() == itemType)
            {
                return child.GetComponent<InventoryItemSlot>();
            }
        }

        return null;
    }



    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in _itemSlotContainer)
        {
            if (child == _itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Item item in _inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(_itemSlotTemplate, _itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(() => {
                // Use item
                _inventory.UseItem(item);

            });

            Image imagem = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            imagem.sprite = item.GetInventorySprite();

            Transform amountTransform = itemSlotRectTransform.Find("Amount");
            if (item.amount > 1)
            {
                TextMeshProUGUI uiText = amountTransform.Find("Text").GetComponent<TextMeshProUGUI>();
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                amountTransform.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        _inventory.OnItemListChanged -= Inventory_OnItemListChanged;
        _inventory.OnItemListAdded -= Inventory_OnItemListAdded;
        _inventory.OnItemListRemoved -= Inventory_OnItemListRemoved;
    }
}
