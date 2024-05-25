using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropInteractable : Interactable
{
    public Item item;
    private Rigidbody2D rb;
    public float gravityDuration = 1f; // Durasi gravitasi setelah item jatuh

    private void Start()
    {
        Debug.Log("ItemDropInteractable: Start method called.");
        rb = GetComponent<Rigidbody2D>(); // Inisialisasi Rigidbody2D
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>(); // Tambahkan Rigidbody2D jika belum ada
            Debug.Log("ItemDropInteractable: Rigidbody2D component added.");
        }
    }

     private void Update()
    {
        
        promptMessage = "Panen Tanaman";

    }

    protected override void Interact()
    {
        Debug.Log("item di ambil.");
        Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem(item.itemName));
        Destroy(gameObject);
    }

    public IEnumerator StopGravity(Rigidbody2D rb, float delay)
    {
        Debug.Log("ItemDropInteractable: StopGravity coroutine started.");
        yield return new WaitForSeconds(delay);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        Debug.Log("ItemDropInteractable: Gravity set to 0.");
    }
}
