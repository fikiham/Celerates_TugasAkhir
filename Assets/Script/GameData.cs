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

    public Dictionary<int, List<SimpleItem>> Storages_ItemNameAndCount = new();

    public GameData()
    {
        int index;
        foreach (Item item in Player_Inventory.Instance.itemList)
        {
            PlayerInventory_ItemNameAndCount.Add(new SimpleItem(item.itemName, item.stackCount, item.Level));
        }

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
