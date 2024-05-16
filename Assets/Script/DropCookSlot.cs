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
        if (slotCook1.item != null && slotCook2.item != null)
        {
            DragCook item1 = slotCook1.item.GetComponent<DragCook>();
            DragCook item2 = slotCook2.item.GetComponent<DragCook>();

            Debug.Log($"Item in SlotCook1: {item1.itemName}");
            Debug.Log($"Item in SlotCook2: {item2.itemName}");

            if ((item1.itemName == "Daging" && item2.itemName == "Cabai") ||
                (item1.itemName == "Cabai" && item2.itemName == "Daging"))
            {
                Debug.Log("Condition met: Daging and Cabai are in the slots.");
                // Buat instance dari DagingPedasPrefab
                GameObject dagingPedas = Instantiate(dagingPedasPrefab, hasilCook.transform);
                dagingPedas.transform.localPosition = Vector3.zero; // Sesuaikan posisi sesuai kebutuhan

                Debug.Log("DagingPedas instance created and positioned.");

                // Set DagingPedas agar dapat dipindahkan
                AddDragHandler(dagingPedas);

                // Hapus item dari SlotCook1 dan SlotCook2
                Destroy(slotCook1.item);
                slotCook1.item = null;
                Destroy(slotCook2.item);
                slotCook2.item = null;

                Debug.Log("Items in SlotCook1 and SlotCook2 destroyed.");
            }
            else
            {
                Debug.Log("Condition not met: The items in the slots are not Daging and Cabai.");
            }
        }
        else
        {
            Debug.Log("Slots are not both filled.");
        }
    }

    private void AddDragHandler(GameObject dagingPedas)
    {
        dagingPedas.AddComponent<DragCook>();
        // Jika DragCook membutuhkan komponen atau pengaturan lain, tambahkan di sini
        Debug.Log("Drag handler added to DagingPedas.");
    }
}
