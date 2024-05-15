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

    public Item GetItem(string name, int count = 1)
    {
        Item itemToGet = items.Find(x => x.itemName == name);
        itemToGet.stackCount = count;
        return Instantiate(itemToGet);
    }
}
