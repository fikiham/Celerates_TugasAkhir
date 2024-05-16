using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookUI : MonoBehaviour
{
    public bool isCookUIPanelOpen = false;
    [SerializeField] Player_Inventory playerInventory;
    [SerializeField] InventoryUI inventoryUI;

    List<Item> Items;
    [Header("Inventory Slot")]
    [SerializeField] Transform itemSlotContainer;
    [SerializeField] Transform itemSlotTemplate;

    [SerializeField] int itemsHorizontalCount = 4;
    [SerializeField] float itemsPadding = 25;

    Vector2 inventoryContainerSize;
    Vector2 itemSizeVector;
    float itemSize;

    private void Update()
    {
        // Close CookUI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCook();
        }
    }

    public void OpenCook()
    {
        GameController.Instance.ShowPersistentUI(false);
        gameObject.SetActive(true);
        isCookUIPanelOpen = true;

        // Tampilkan inventaris di panel CookUI saat dibuka
        inventoryUI.SetInventory(Player_Inventory.Instance.itemList);
    }

    public void CloseCook()
    {
        GameController.Instance.ShowPersistentUI(true);
        gameObject.SetActive(false);
        isCookUIPanelOpen = false;
    }




}
