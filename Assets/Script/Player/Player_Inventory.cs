using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Player_Inventory : MonoBehaviour // Handle Player Inventory with InventoryUI as UI Helper
{
    public static Player_Inventory Instance; // Access this class by its Instance

    public List<Item> itemList;

    [Header("UI ELEMENTS")]
    [SerializeField] Item emptyItem;
    [HideInInspector] public Item equippedCombat;
    [SerializeField] Image equippedUI;
    [HideInInspector] public List<Item> quickSlots = new List<Item>(2);
    [SerializeField] List<Image> quickSlotsUI;

    KeyCode openInventoryInput = KeyCode.B;
    [SerializeField] GameObject inventoryGO;
    InventoryUI inventoryUI;
    bool inventoryOpened;

    private void Awake()
    {
        Instance = this;

        // Making sure everything in it is a clone
        List<Item> newList = new();
        foreach (Item item in itemList)
        {
            newList.Add(Instantiate(item));
        }
        itemList.Clear();
        itemList = newList;

        emptyItem = Instantiate(emptyItem);


        inventoryUI = inventoryGO.GetComponent<InventoryUI>();
    }
    private void Start()
    {
        // Handle so that equipped items never null
        EquipItem(emptyItem);
        AddQuickSlot(emptyItem, 0);
        AddQuickSlot(emptyItem, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryInput) && !GameController.Instance.gamePaused)
        {
            inventoryOpened = !inventoryOpened;
            inventoryGO.SetActive(inventoryOpened);
            GameController.Instance.ShowPersistentUI(!inventoryOpened);

            inventoryUI.HandleItemsSize();
            inventoryUI.SetInventory(itemList);
            if (itemList.Count > 0)
                inventoryUI.SetDescription(itemList[0]);
            else
                inventoryUI.SetDescription(emptyItem);
        }


        if (Input.GetKeyDown(KeyCode.Escape) && inventoryOpened)
        {
            inventoryGO.SetActive(false);
            GameController.Instance.ShowPersistentUI(true);
            inventoryOpened = false;
        }


    }

    public void AddItem(Item item)
    {
        item = Instantiate(item);
        if (item.isStackable && itemList.Exists(x => x.itemName == item.itemName))
        {
            itemList.Find(x => x.itemName == item.itemName).stackCount++;
        }
        else
        {
            itemList.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        item = Instantiate(item);
        if (item.isStackable)
        {
            itemList.Find(x => x.itemName == item.itemName).stackCount--;
            if (itemList.Find(x => x.itemName == item.itemName).stackCount <= 0)
                itemList.Remove(itemList.Find(x => x.itemName == item.itemName));
        }
        else
            itemList.Remove(itemList.Find(x => x.itemName == item.itemName));
    }

    public void EquipItem(Item item)
    {
        if (!itemList.Contains(item) && item.itemName != "Empty")
            return;
        equippedCombat = item;
        equippedUI.sprite = item.sprite;
        inventoryUI.SetActiveItem(0, item);
    }

    // Add item to quick slot according index (0,1)
    public void AddQuickSlot(Item item, int index)
    {
        if (!itemList.Contains(item) && item.itemName != "Empty")
            return;

        switch (item.type)
        {
            case ItemType.Heal:
            case ItemType.Buff:
                break;
            default: break;
        }

        quickSlots[index] = item;
        quickSlotsUI[index].sprite = item.sprite;
        inventoryUI.SetActiveItem(index + 1, item);
    }


    // Use which quick slot (1,2)
    public void UseQuickSlot(int which)
    {
        // Making sure there is an item in the quick slot
        Item item = quickSlots[which - 1];
        if (item == null || item.itemName == "Empty")
        {
            print("No item bish");
            return;
        }

        // Using them from inventory
        print("using quick slot " + (which - 1));
        item = itemList.Find(x => x.itemName == item.itemName);
        item.stackCount--;
        if (item.stackCount <= 0)
        {
            itemList.Remove(item);
            AddQuickSlot(emptyItem, which - 1);
        }

        // Have its effect
        switch (item.type)
        {
            case ItemType.Heal:
                // Heal Player
                print("HEALED");
                Player_Health.Instance.Heal(10);
                break;

            case ItemType.Buff:
                // Buff player
                print("BUFFED");
                break;

            default: break;
        }
    }
}
