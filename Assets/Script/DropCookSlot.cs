using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropCookSlot : MonoBehaviour, IDropHandler
{
    public GameObject item; // Item yang saat ini ada di slot
    public string slotName; // Nama slot, misalnya "SlotCook1" atau "SlotCook2"

    public GameObject hasilCook; // Referensi ke GameObject HasilCook
    public GameObject dagingPedasPrefab; // Prefab untuk DagingPedas

    [SerializeField] CookUI cookUI;

    private Button hasilCookButton;

    private void Start()
    {
        // Menetapkan inventorySlots dari CookUI jika belum dilakukan
        if (cookUI == null || cookUI.inventorySlots == null)
        {
            Debug.LogError("CookUI or inventorySlots in CookUI is not assigned!");
        }

        // Mendapatkan komponen Button dari hasilCook
        hasilCookButton = hasilCook.GetComponent<Button>();
        if (hasilCookButton == null)
        {
            Debug.LogError("Button component on hasilCook is not assigned!");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedItem = eventData.pointerDrag;
        DragCook dragCook = draggedItem.GetComponent<DragCook>();

        if (dragCook != null)
        {
            if (!item)
            {
                RemoveFromPreviousSlot(draggedItem);
                SetItemToSlot(draggedItem);
                CheckAndCreateDagingPedas();
            }
            else
            {
                dragCook.ResetPosition();
            }
        }
    }

    private void RemoveFromPreviousSlot(GameObject draggedItem)
    {
        DropCookSlot[] slots = transform.parent.GetComponentsInChildren<DropCookSlot>();
        foreach (DropCookSlot slot in slots)
        {
            if (slot.item == draggedItem)
            {
                slot.item = null;
            }
        }
    }

    private void SetItemToSlot(GameObject draggedItem)
    {
        RectTransform itemRectTransform = draggedItem.GetComponent<RectTransform>();
        itemRectTransform.SetParent(transform);
        itemRectTransform.anchoredPosition = Vector3.zero;
        item = draggedItem;
    }

    private void CheckAndCreateDagingPedas()
    {
        if (cookUI.slotCook1.item != null && cookUI.slotCook2.item != null)
        {
            DragCook item1 = cookUI.slotCook1.item.GetComponent<DragCook>();
            DragCook item2 = cookUI.slotCook2.item.GetComponent<DragCook>();

            if ((item1.itemName == "Cabbage" && item2.itemName == "Bear Meat") ||
                (item1.itemName == "Bear Meat" && item2.itemName == "Cabbage"))
            {
                CreateDagingPedas();
            }
            else
            {
                CancelDagingPedas();
            }
        }
        else
        {
            CancelDagingPedas();
        }
    }

    private void CreateDagingPedas()
    {
        CancelDagingPedas();

        GameObject dagingPedas = Instantiate(dagingPedasPrefab, hasilCook.transform);
        dagingPedas.transform.localPosition = Vector3.zero;
        dagingPedas.name = "Wolf Meat";

        hasilCookButton.onClick.RemoveAllListeners();
        hasilCookButton.onClick.AddListener(() =>
        {
            Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem(dagingPedas.name));
            Destroy(dagingPedas);
            RemoveItemsFromSlots();
        });
    }

    public void CancelDagingPedas()
    {
        if (hasilCook.transform.childCount > 0)
        {
            Destroy(hasilCook.transform.GetChild(0).gameObject);
        }
        hasilCookButton.onClick.RemoveAllListeners();
    }

    private void RemoveItemsFromSlots()
    {
        if (cookUI.slotCook1.item != null)
        {
            Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem(cookUI.slotCook1.item.GetComponent<DragCook>().itemName));
            Destroy(cookUI.slotCook1.item);
            cookUI.slotCook1.item = null;
        }

        if (cookUI.slotCook2.item != null)
        {
            Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem(cookUI.slotCook2.item.GetComponent<DragCook>().itemName));
            Destroy(cookUI.slotCook2.item);
            cookUI.slotCook2.item = null;
        }

        cookUI.RefreshSlots();
        CheckAndCreateDagingPedas(); // Memperbarui daging pedas setelah item dihapus
    }

    public void ReturnToInventory()
    {
        if (item != null)
        {
            item.transform.SetParent(cookUI.inventorySlots.transform);
            item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            item = null;

            CheckAndCreateDagingPedas();
        }
    }
}
