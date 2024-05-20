using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string playerName;

    #region ITEM_STUFF

    [System.Serializable]
    public class SimpleItem
    {
        public string itemName;
        public int stackCount;
        public int level;

        public SimpleItem(string itemName, int stackCount, int level)
        {
            this.itemName = itemName;
            this.stackCount = stackCount;
            this.level = level;
        }
    }
    public List<SimpleItem> PlayerInventory_ItemNameAndCount = new();
    public SimpleItem[] PlayerInventory_ActiveItemAndCount = new SimpleItem[4];

    public Dictionary<int, List<SimpleItem>> Storages_ItemNameAndCount = new();

    #endregion

    public GameData()
    {
        Player_Inventory inventory = Player_Inventory.Instance;

        playerName = GameController.Instance.playerName;

        int index;
        // Save Player Inventory Items
        foreach (Item item in inventory.itemList)
        {
            PlayerInventory_ItemNameAndCount.Add(new SimpleItem(item.itemName, item.stackCount, item.Level));
        }

        // Save Player Active Items
        PlayerInventory_ActiveItemAndCount[0] = (new(inventory.equippedCombat[0].itemName, 1, inventory.equippedCombat[0].Level));
        PlayerInventory_ActiveItemAndCount[1] = (new(inventory.equippedCombat[1].itemName, inventory.equippedCombat[1].stackCount, inventory.equippedCombat[1].Level));
        PlayerInventory_ActiveItemAndCount[2] = (new(inventory.quickSlots[0].itemName, inventory.quickSlots[0].stackCount, 1));
        PlayerInventory_ActiveItemAndCount[3] = (new(inventory.quickSlots[1].itemName, inventory.quickSlots[1].stackCount, 1));

        // Save Storages 
        index = 0;
        foreach (StorageInteractable storage in StorageSystem.Instance.GetStorages())
        {
            List<SimpleItem> items = new();
            foreach (Item item in storage.Items)
            {
                items.Add(new SimpleItem(item.itemName, item.stackCount, item.Level));
            }
            Storages_ItemNameAndCount.Add(index, items);
            index++;
        }
    }
}
