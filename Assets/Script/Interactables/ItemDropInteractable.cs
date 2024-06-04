using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropInteractable : Interactable
{
    public Item item;
    private Rigidbody2D rb;
    public float gravityDuration = 2f; // Durasi gravitasi setelah item jatuh

    private void Start()
    {
        Debug.Log("ItemDropInteractable: Start method called.");
    }

    protected override void Interact()
    {
        SoundManager.Instance.PlaySound("Pick");
        Debug.Log(item.itemName + " di ambil.");
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
