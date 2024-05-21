using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance;

    [SerializeField] List<Item> items;

    private void Awake()
    {
        Instance = this;
    }

   public Item GetItem(string name, int count = 1, int level = 1)
{
    Item itemToGet = items.Find(x => x.itemName == name);
    if (itemToGet != null)
    {
        itemToGet.stackCount = count;
        itemToGet.Level = level;
        return Instantiate(itemToGet);
    }
    else
    {
        Debug.LogWarning($"Item with name {name} not found in ItemPool!");
        return null;
    }
}

}
