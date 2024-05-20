using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopUI : MonoBehaviour
{
    [SerializeField] TMP_Text currentMoney;

    [Header("BUY UI")]
    [SerializeField] Transform itemsToBuyContainer;
    [SerializeField] Transform itemsToBuyTemplate;
    public List<Item> itemsToBuy;
    [SerializeField] int buyHorizontalCount = 4;
    [SerializeField] float buyPadding = 15;


    [Header("SELL UI")]
    [SerializeField] Transform itemsToSellContainer;
    [SerializeField] Transform itemsToSellTemplate;
    public List<Item> itemsToSell;
    [SerializeField] int sellHorizontalCount = 4;
    [SerializeField] float sellPadding = 15;

    [Header("UPGRADE UI")]
    [SerializeField] Transform itemsToUpgradeContainer;
    [SerializeField] Transform itemsToUpgradeTemplate;
    public List<Item> itemsToUpgrade;
    [SerializeField] int upgradeHorizontalCount = 1;
    [SerializeField] float upgradePadding = 10;

    private void Update()
    {
        // Close Shop
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            GameController.Instance.ShowPersistentUI(true);
        }
    }

    public void OpenShop()
    {
        GameController.Instance.ShowPersistentUI(false);
        gameObject.SetActive(true);
        currentMoney.text = "$" + GameEconomy.Instance.Money;

        itemsToSell = Player_Inventory.Instance.itemList;
        itemsToUpgrade.Clear();
        foreach (Item item in itemsToSell)
        {
            switch (item.type)
            {
                case ItemType.Melee_Combat:
                case ItemType.Ranged_Combat:
                    itemsToUpgrade.Add(item);
                    break;
                default: break;
            }
        }

        List<Item> temp = new(itemsToBuy);
        itemsToBuy.Clear();
        foreach (Item item in temp)
        {
            AddToStore(item);
        }

        RefreshListUI(itemsToBuy, itemsToBuyContainer, itemsToBuyTemplate, buyHorizontalCount, buyPadding);
        RefreshListUI(itemsToSell, itemsToSellContainer, itemsToSellTemplate, sellHorizontalCount, sellPadding, buySellOrUpgrade: 2);
        RefreshListUI(itemsToUpgrade, itemsToUpgradeContainer, itemsToUpgradeTemplate, upgradeHorizontalCount, upgradePadding, 3);
    }


    void RefreshListUI(List<Item> Items, Transform container, Transform template, int horizontalCount, float itemsPadding, int buySellOrUpgrade = 1)
    {
        foreach (Transform child in container)
        {
            if (child == template) continue;
            Destroy(child.gameObject);
        }
        Vector2 containerSize = container.GetComponent<RectTransform>().sizeDelta;
        float itemSize = (containerSize.x / horizontalCount) - itemsPadding;
        Vector2 itemSizeVector = Vector2.one * itemSize;
        float itemSlotCellSize = itemSize + itemsPadding;
        float correction = itemSize / 2;
        int x = 0, y = 0;
        foreach (Item item in Items)
        {
            Transform itemInInventory = Instantiate(template, container);
            itemInInventory.gameObject.SetActive(true);
            itemInInventory.name = item.itemName;
            itemInInventory.GetChild(0).GetComponent<Image>().sprite = item.sprite;
            itemInInventory.GetChild(1).GetComponent<TMP_Text>().text = item.stackCount.ToString();

            if (buySellOrUpgrade == 1 && item.stackCount == 0)
            {
                itemInInventory.GetChild(2).GetComponent<Button>().interactable = false;
                itemInInventory.GetChild(2).GetComponentInChildren<TMP_Text>().text = "SOLD";
            }

            RectTransform itemSlotRectTransform = itemInInventory.GetComponent<RectTransform>();
            if (buySellOrUpgrade != 3)
            {
                itemSlotRectTransform.sizeDelta = itemSizeVector;
                itemSlotRectTransform.anchoredPosition = new((x * itemSlotCellSize) + correction, (y * -itemSlotCellSize) - correction);
            }
            else
                itemSlotRectTransform.anchoredPosition = new((x * itemSlotCellSize) + correction, (y * -template.GetComponent<RectTransform>().sizeDelta.y + itemsPadding) - template.GetComponent<RectTransform>().sizeDelta.y / 1.5f);

            itemInInventory.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            switch (buySellOrUpgrade)
            {
                case 1:
                    itemInInventory.GetComponentInChildren<Button>().onClick.AddListener(() => BuyItem(item));
                    break;
                case 2:
                    itemInInventory.GetComponentInChildren<Button>().onClick.AddListener(() => SellItem(item));
                    break;
                case 3:
                    itemInInventory.GetComponentInChildren<Button>().onClick.AddListener(() => UpgradeItem(item));
                    break;
            }
            x++;
            if (x >= horizontalCount)
            {
                x = 0;
                y++;
            }
        }
    }

    void AddToStore(Item item)
    {
        item = Instantiate(item);
        if (itemsToBuy.Exists(x => x.itemName == item.itemName))
        {
            if (item.isStackable)
                itemsToBuy.Find(x => x.itemName == item.itemName).stackCount++;
            else if (!item.isStackable && itemsToBuy.Find(x => x.itemName == item.itemName).stackCount == 0)
                itemsToBuy.Find(x => x.itemName == item.itemName).stackCount++;
            else
                itemsToBuy.Add(item);
        }
        else
            itemsToBuy.Add(item);
    }


    // Usable Function for buying, selling, and upgrading items
    void BuyItem(Item item)
    {
        // Check if can buy then subtract money
        if (GameEconomy.Instance.SpendMoney(item.BuyValue))
        {
            // Add Item to inventory
            Player_Inventory.Instance.AddItem(item);
            // Remove/Disable from shop
            item.stackCount--;
        }
        OpenShop();
    }
    void SellItem(Item item)
    {
        // Add money
        GameEconomy.Instance.GainMoney(item.SellValue);
        // Sell how many
        // Remove from item
        Player_Inventory.Instance.RemoveItem(item);
        // Add item to shop stock ???
        if (item.stackCount <= 0) // sold item count will be zero
            item.stackCount = 1; // so make it 1 in the store
        AddToStore(item);
        OpenShop();
    }
    void UpgradeItem(Item item)
    {
        // Check if money enough then subtract
        if (GameEconomy.Instance.SpendMoney(item.UpgradeCost))
            item.Level++; // Add item level
        OpenShop();

    }
}
