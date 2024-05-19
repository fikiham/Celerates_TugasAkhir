using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public bool isReadyToHarvest = false;
    public Sprite[] growthSprites; // Array sprite untuk setiap tahap pertumbuhan
    private bool isGrowing = true;
    private int growthCount = 0;
    private int maxGrowthCount = 2; // Jumlah pertumbuhan maksimum sebelum panen
    private float growthTime = 10f; // Waktu pertumbuhan dalam detik

    private SpriteRenderer spriteRenderer;
    public GameObject hasilPanenPrefab; // Prefab hasil panen (buah cabai)

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        while (isGrowing)
        {
            yield return new WaitForSeconds(growthTime); // Tunggu 10 detik

            if (growthCount < maxGrowthCount)
            {
               
                growthCount++;

                // Ubah sprite setiap kali biji tumbuh
                if (growthCount < growthSprites.Length)
                {
                    spriteRenderer.sprite = growthSprites[growthCount];
                   
                }

                if (growthCount == maxGrowthCount)
                {
                    isReadyToHarvest = true;
                  
                    isGrowing = false; // Berhenti pertumbuhan setelah biji siap panen
                }
            }
        }
    }

    public void Harvest()
    {
        if (isReadyToHarvest)
        {
            Debug.Log("Biji dipanen!");
            // Instantiate hasil panen (buah cabai) sedikit di atas tanaman
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0); // Mengatur posisi sedikit di atas tanaman
            GameObject hasilPanen = Instantiate(hasilPanenPrefab, spawnPosition, Quaternion.identity);

            // Tambahkan Rigidbody2D jika belum ada
            Rigidbody2D rb = hasilPanen.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = hasilPanen.AddComponent<Rigidbody2D>();
            }

            // Atur gravityScale kecil untuk efek jatuh ringan
            rb.gravityScale = 1f;
          

            // Tambahkan sedikit force untuk gerakan jatuh
            rb.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), -1f), ForceMode2D.Impulse);

            // Hentikan gravitasi setelah beberapa waktu
            StartCoroutine(StopGravity(rb, 0.5f)); // Hentikan gravitasi setelah 0.5 detik

            // Atur ulang fase pertumbuhan ke awal
            growthCount = 0;
            spriteRenderer.sprite = growthSprites[growthCount];
            isReadyToHarvest = false;
            isGrowing = true; // Mulai pertumbuhan kembali
            StartCoroutine(Grow());
        }
        else
        {
            Debug.Log("Biji belum siap dipanen!");
        }
    }

    private IEnumerator StopGravity(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
       
    }
}
