using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropInteractable : Interactable
{
    public Item item;

    protected override void Interact()
    {
        Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem(item.itemName));
    }
}
