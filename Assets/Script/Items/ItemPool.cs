using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance;

    [SerializeField] GameObject itemDropPrefab;

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

    public void DropItem(string itemName, Vector2 pos, int count = 1, int level = 1)
    {
        GameObject droppedItem = Instantiate(itemDropPrefab, pos, Quaternion.identity);
        droppedItem.GetComponent<SpriteRenderer>().sprite = GetItem(itemName).sprite;

        // Tambahkan komponen Rigidbody2D jika belum ada
        Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = droppedItem.AddComponent<Rigidbody2D>();
        }

        // Atur gravityScale kecil untuk efek jatuh ringan
        rb.gravityScale = 0.5f;

        // Tambahkan sedikit force untuk gerakan jatuh
        rb.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), -1f), ForceMode2D.Impulse);

        // Hentikan gravitasi setelah beberapa waktu
        StartCoroutine(StopGravity(rb, 0.5f)); // Hentikan gravitasi setelah 0.5 detik

        droppedItem.GetComponent<ItemDropInteractable>().item = GetItem(itemName, count, level);
    }

    private IEnumerator StopGravity(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }
}
