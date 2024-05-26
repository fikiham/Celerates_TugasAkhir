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
    Player_Action pA;

    public List<Item> itemList;

    KeyCode switchWeapon = KeyCode.R;
    [HideInInspector] public bool meleeOrRanged = true;

    [Header("UI ELEMENTS")]
    public Item emptyItem;
    [HideInInspector] public List<Item> equippedCombat = new(2);
    [HideInInspector] public Item equippedWeapon;
    [HideInInspector] public List<Item> quickSlots = new(2);

    KeyCode openInventoryInput = KeyCode.B;
    InventoryUI inventoryUI;
    bool inventoryOpened;

    private void Awake()
    {
        Instance = this;
        pA = GetComponent<Player_Action>();

        // Making sure everything in it is a clone
        List<Item> newList = new();
        foreach (Item item in itemList)
        {
            newList.Add(Instantiate(item));
        }
        itemList.Clear();
        itemList = newList;

        emptyItem = Instantiate(emptyItem);

    }
    private void Start()
    {
        inventoryUI = PlayerUI.Instance.inventoryUI.GetComponent<InventoryUI>();
        // Handle so that equipped items never null
        EquipItem(emptyItem, 0);
        EquipItem(emptyItem, 1);
        AddQuickSlot(emptyItem, 0);
        AddQuickSlot(emptyItem, 1);
    }

    private void Update()
    {
        if (GameController.Instance.enablePlayerInput)
        {
            // Open inventory on open inventory input (B)
            if (Input.GetKeyDown(openInventoryInput))
            {
                inventoryOpened = !inventoryOpened;
                PlayerUI.Instance.inventoryUI.SetActive(inventoryOpened);
                GameController.Instance.ShowPersistentUI(!inventoryOpened);
                GameController.Instance.gamePaused = inventoryOpened;

                inventoryUI.SetInventory(itemList);
                if (itemList.Count > 0)
                    inventoryUI.SetDescription(itemList[0]);
                else
                    inventoryUI.SetDescription(emptyItem);
            }

            // Close inventory with escape when opened
            if (Input.GetKeyDown(KeyCode.Escape) && inventoryOpened)
            {
                PlayerUI.Instance.inventoryUI.SetActive(false);
                GameController.Instance.ShowPersistentUI(true);
                inventoryOpened = false;
                GameController.Instance.gamePaused = false;
            }

            // Key to switch weapon
            if (Input.GetKeyDown(switchWeapon))
            {
                meleeOrRanged = !meleeOrRanged;
            }
        }

        // Use of different weapon
        if (meleeOrRanged)
        {
            equippedWeapon = equippedCombat[0];
            PlayerUI.Instance.equippedUI.sprite = equippedWeapon.sprite;
        }
        else
        {
            equippedWeapon = equippedCombat[1];
            PlayerUI.Instance.equippedUI.sprite = equippedWeapon.sprite;
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

    public Item FindItemInInventory(string name)
    {
        if (itemList.Exists(x => x.itemName == name))
        {
            return itemList.Find(x => x.itemName == name);
        }
        return ItemPool.Instance.GetItem("Empty");
    }

    public void EquipItem(Item item, int index)
    {
        if (!itemList.Exists(x => x.itemName == item.itemName) && item.itemName != "Empty")
            return;
        equippedCombat[index] = item;
        PlayerUI.Instance.inventoryUI.GetComponent<InventoryUI>().SetActiveItem(index, item);
    }

    // Add item to quick slot according index (0,1)
    public void AddQuickSlot(Item item, int index)
    {
        if (!itemList.Exists(x => x.itemName == item.itemName) && item.itemName != "Empty")
            return;

        switch (item.type)
        {
            case ItemType.Heal:
            case ItemType.Buff:
                break;
            default: break;
        }

        quickSlots[index] = item;
        PlayerUI.Instance.quickSlotsUI_Inventory[index].sprite = item.sprite;
        PlayerUI.Instance.inventoryUI.GetComponent<InventoryUI>().SetActiveItem(index + 2, item);
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
