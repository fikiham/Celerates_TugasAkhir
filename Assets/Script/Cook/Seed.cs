using System.Collections;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public bool isReadyToHarvest = false;
    public bool siram = false;
    public Sprite[] growthSprites; // Array sprite untuk setiap tahap pertumbuhan
    private bool isGrowing = true;
    private int growthCount = 0;
    private int maxGrowthCount = 2; // Jumlah pertumbuhan maksimum sebelum panen
    private float growthTime = 10f; // Waktu pertumbuhan dalam detik

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        while (isGrowing)
        {
            yield return new WaitForSeconds(growthTime); // Tunggu growthTime detik

            if (growthCount < maxGrowthCount)
            {
                growthCount++;

                // Ubah sprite setiap kali biji tumbuh
                if (growthCount < growthSprites.Length)
                {
                    spriteRenderer.sprite = growthSprites[growthCount];
                }

                // Pindahkan objek ke posisi tertentu pada growthCount

                if (growthCount == 1)

                {
                    Vector3 newPosition = transform.position;
                    newPosition.y += 0.83f;
                    transform.position = newPosition;
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
            // Drop item menggunakan ItemPool
            ItemPool.Instance.DropItem("BuahCabai", transform.position + new Vector3(0, 0.5f, 0));
            // Atur ulang fase pertumbuhan ke awal dan lain-lain...

            growthCount = 0;
            spriteRenderer.sprite = growthSprites[growthCount];
            isReadyToHarvest = false;
            siram = true;

            // Reset posisi y
            Vector3 newPosition = transform.position;
            newPosition.y -= 0.83f * (maxGrowthCount - 1); // Reset posisi sesuai dengan growthCount maksimum
            transform.position = newPosition;
        }
        else if (siram)
        {
            Debug.Log("Siram dulu bos");
        }
        else
        {
            Debug.Log("Biji belum siap dipanen bos");
        }
    }

    public void Siram()
    {
        if (siram == true)
        {
            isGrowing = true; // Mulai pertumbuhan kembali
            StartCoroutine(Grow());
            siram = false;
        }
        else
        {
            Debug.Log("bukan waktunya siram bro ");
        }
    }
}
