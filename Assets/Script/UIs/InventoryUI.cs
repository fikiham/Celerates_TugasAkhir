using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour // Attach this to the InventoryUI prefab
{
    List<Item> Items;

    [Header("Active Slot")]
    [SerializeField] Transform equippedItem1;
    [SerializeField] Transform equippedItem2;
    [SerializeField] Transform quickSlot1;
    [SerializeField] Transform quickSlot2;


    [Header("Inventory Slot")]
    [SerializeField] Transform itemSlotContainer;
    [SerializeField] Transform itemSlotTemplate;

    [SerializeField] int itemsHorizontalCount = 4;
    [SerializeField] float itemsPadding = 25;

    Vector2 inventoryContainerSize;
    Vector2 itemSizeVector;
    float itemSize;

    [Header("Item Description")]
    [SerializeField] Image itemSprite;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemDesc;
    [SerializeField] Button itemAction;


    private void Start()
    {
        HandleItemsSize();
    }

    // Handle equipped items
    public void SetActiveItem(int slot, Item item)
    {
        Transform pickedSlot;
        switch (slot)
        {
            case 0: pickedSlot = equippedItem1; break;
            case 1: pickedSlot = equippedItem2; break;
            case 2: pickedSlot = quickSlot1; break;
            case 3: pickedSlot = quickSlot2; break;
            default: pickedSlot = equippedItem1; break;
        }

        pickedSlot.GetChild(0).GetComponent<Image>().sprite = item.sprite;
        pickedSlot.GetChild(1).GetComponent<TMP_Text>().text = item.stackCount <= 0 ? "" : item.stackCount.ToString();
    }

    // Handle opened inventory with its items
    public void SetInventory(List<Item> Items)
    {
        this.Items = Items;
        RefreshInventoryItems();
    }

    void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        float itemSlotCellSize = itemSize + itemsPadding;
        float correction = itemSize / 2;
        int x = 0, y = 0;
        foreach (Item item in Items)
        {
            Transform itemInInventory = Instantiate(itemSlotTemplate, itemSlotContainer);
            RectTransform itemSlotRectTransform = itemInInventory.GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.sizeDelta = itemSizeVector;
            itemSlotRectTransform.gameObject.name = item.itemName;
            itemSlotRectTransform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
            itemSlotRectTransform.GetChild(1).GetComponent<TMP_Text>().text = item.stackCount.ToString();
            itemSlotRectTransform.anchoredPosition = new((x * itemSlotCellSize) + correction, (y * -itemSlotCellSize) - correction);

            itemInInventory.GetComponent<Button>().onClick.RemoveAllListeners();
            itemInInventory.GetComponent<Button>().onClick.AddListener(() => SetDescription(item));
            x++;
            if (x >= itemsHorizontalCount)
            {
                x = 0;
                y++;
            }
        }
    }

    // Function Helper for UI
    public void HandleItemsSize()
    {
        inventoryContainerSize = itemSlotContainer.GetComponent<RectTransform>().sizeDelta;
        itemSize = (inventoryContainerSize.x / itemsHorizontalCount) - itemsPadding;
        itemSizeVector = Vector2.one * itemSize;
    }

    public void SetDescription(Item item)
    {
        // Set item's texts
        itemSprite.sprite = item.sprite;
        itemName.text = item.itemName;
        itemDesc.text = item.itemDescription;

        // Set the "equip" button functionality
        itemAction.onClick.RemoveAllListeners();
        switch (item.type)
        {
            case ItemType.Melee_Combat:
                itemAction.onClick.AddListener(() => Player_Inventory.Instance.EquipItem(item,0));
                break;
            case ItemType.Ranged_Combat:
                itemAction.onClick.AddListener(() => Player_Inventory.Instance.EquipItem(item,1));
                break;
            case ItemType.Heal:
                itemAction.onClick.AddListener(() => Player_Inventory.Instance.AddQuickSlot(item, 0));
                break;
            case ItemType.Buff:
                itemAction.onClick.AddListener(() => Player_Inventory.Instance.AddQuickSlot(item, 1));
                break;
            default: break;
        }

        // Set the "Equip" button according to item's type
        string itemUses;
        if (item.type == ItemType.Item)
        {
            itemUses = "CAN'T EQUIP";
            itemAction.interactable = false;
        }
        else
        {
            itemUses = "EQUIP";
            itemAction.interactable = true;
        }
        itemAction.GetComponentInChildren<TMP_Text>().text = itemUses;

    }
}
