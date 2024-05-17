using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropCookSlot : MonoBehaviour, IDropHandler
{
    public GameObject item; // Item yang saat ini ada di slot
    public string slotName; // Nama slot, misalnya "SlotCook1" atau "SlotCook2"

    public GameObject hasilCook; // Referensi ke GameObject HasilCook
    public GameObject dagingPedasPrefab; // Prefab untuk DagingPedas

    private DropCookSlot slotCook1;
    private DropCookSlot slotCook2;

    [SerializeField] CookUI cookUI;

    private void Start()
    {
        // Assuming the slots are siblings, find SlotCook1 and SlotCook2
        DropCookSlot[] slots = transform.parent.GetComponentsInChildren<DropCookSlot>();
        foreach (DropCookSlot slot in slots)
        {
            if (slot.slotName == "SlotCook1")
            {
                slotCook1 = slot;
            }
            else if (slot.slotName == "SlotCook2")
            {
                slotCook2 = slot;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Mengambil referensi dari item yang di-drop
        GameObject draggedItem = eventData.pointerDrag;
        DragCook dragCook = draggedItem.GetComponent<DragCook>();

        if (dragCook != null)
        {
            // Mengecek apakah slot sudah terisi
            if (!item)
            {
                // Menghapus item dari slot sebelumnya jika ada
                RemoveFromPreviousSlot(draggedItem);

                // Mengatur item ke slot ini
                SetItemToSlot(draggedItem);

                // Cek apakah kondisi untuk membuat DagingPedas terpenuhi
                CheckAndCreateDagingPedas();
            }
            else
            {
                // Slot tidak kosong, ganti dengan item yang di-drop
                RemoveFromPreviousSlot(draggedItem);
                SetItemToSlot(draggedItem);
                CheckAndCreateDagingPedas();
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
        Button hasilCookButton = hasilCook.GetComponent<Button>();
        if (slotCook1.item != null && slotCook2.item != null)
        {
            DragCook item1 = slotCook1.item.GetComponent<DragCook>();
            DragCook item2 = slotCook2.item.GetComponent<DragCook>();

            Debug.Log($"Item in SlotCook1: {item1.itemName}");
            Debug.Log($"Item in SlotCook2: {item2.itemName}");

            if ((item1.itemName == "Cabbage" && item2.itemName == "Bear Meat") ||
                (item1.itemName == "Bear Meat" && item2.itemName == "Cabbage"))
            {
                Debug.Log("Condition met: Daging and Cabai are in the slots.");
                // Buat instance dari DagingPedasPrefab
                GameObject dagingPedas = Instantiate(dagingPedasPrefab, hasilCook.transform);
                dagingPedas.transform.localPosition = Vector3.zero; // Sesuaikan posisi sesuai kebutuhan
                dagingPedas.name = "Wolf Meat";

                Debug.Log("DagingPedas instance created and positioned.");



                hasilCookButton.onClick.RemoveAllListeners();
                // Add Item to inventory
                hasilCookButton.onClick.AddListener(() => Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem(dagingPedas.name)));

                // Remove Everything UI
                hasilCookButton.onClick.AddListener(() => Destroy(dagingPedas));
                hasilCookButton.onClick.AddListener(() => Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem(item1.itemName)));
                hasilCookButton.onClick.AddListener(() => Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem(item2.itemName)));
                hasilCookButton.onClick.AddListener(() => Destroy(slotCook1.item));
                hasilCookButton.onClick.AddListener(() => slotCook1.item = null);
                hasilCookButton.onClick.AddListener(() => Destroy(slotCook2.item));
                hasilCookButton.onClick.AddListener(() => slotCook2.item = null);
                hasilCookButton.onClick.AddListener(() => cookUI.RefreshSlots());
                hasilCookButton.onClick.AddListener(() => hasilCookButton.onClick.RemoveAllListeners());

                Debug.Log("Items in SlotCook1 and SlotCook2 destroyed.");
            }
            else
            {
                Destroy(hasilCook.transform.GetChild(0).gameObject!);
                hasilCookButton.onClick.RemoveAllListeners();
                Debug.Log("Condition not met: The items in the slots are not Daging and Cabai.");
            }
        }
        else
        {
            Destroy(hasilCook.transform.GetChild(0).gameObject!);
            hasilCookButton.onClick.RemoveAllListeners();
            Debug.Log("Slots are not both filled.");
        }
    }
}
