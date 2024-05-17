using System.Collections.Generic;

[System.Serializable]
public class GameData
{
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

    public GameData()
    {
        int index;
        // Save Player Inventory Items
        foreach (Item item in Player_Inventory.Instance.itemList)
        {
            PlayerInventory_ItemNameAndCount.Add(new SimpleItem(item.itemName, item.stackCount, item.Level));
        }

        // Save Player Active Items
        PlayerInventory_ActiveItemAndCount[0] = (new(Player_Inventory.Instance.equippedCombat[0].itemName, 1, Player_Inventory.Instance.equippedCombat[0].Level));
        PlayerInventory_ActiveItemAndCount[1] = (new(Player_Inventory.Instance.equippedCombat[1].itemName, 1, Player_Inventory.Instance.equippedCombat[1].Level));
        PlayerInventory_ActiveItemAndCount[2] = (new(Player_Inventory.Instance.quickSlots[0].itemName, Player_Inventory.Instance.quickSlots[0].stackCount, 1));
        PlayerInventory_ActiveItemAndCount[3] = (new(Player_Inventory.Instance.quickSlots[1].itemName, Player_Inventory.Instance.quickSlots[1].stackCount, 1));

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
