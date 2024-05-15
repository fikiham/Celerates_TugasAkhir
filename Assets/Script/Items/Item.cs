using UnityEngine;


public enum ItemType
{
    Combat,
    Heal,
    Buff,
    Item
}

[CreateAssetMenu(menuName = "Make an Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public Sprite sprite;
    [TextArea]
    public string itemDescription;

    // Combat Item
    public int Level;
    public int Damage;
    public int UpgradeCost;

    // Regular Item
    public bool isStackable = false;
    public int stackCount;
    public int BuyValue;
    public int SellValue;
}
