using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookIngredients : MonoBehaviour
{
    [SerializeField] CookUI cookUI; // Referensi ke skrip CookUI
    [SerializeField] Transform parentTransform; // Parent untuk meletakkan hasil resep
    [SerializeField] Color backgroundColor = new Color(0.85f, 0.85f, 0.85f); // Warna pembungkus (D9D9D9 dalam heksadesimal)

    [SerializeField] GameObject hasilCook; // Menghubungkan hasil cook ke CookIngredients
    [SerializeField] GameObject slotCook1; // Menghubungkan slot cook 1 ke CookIngredients
    [SerializeField] GameObject slotCook2; // Menghubungkan slot cook 2 ke CookIngredients
    [SerializeField] GameObject slotCook3; // Menghubungkan slot cook 3 ke CookIngredients
    
    void Start()
    {
        CekIngredients();
    }

    public void CekIngredients()
    {
        Debug.Log("Fungsi CekIngredients dijalankan");

        // Pastikan cookUI dan parentTransform telah diisi
        if (cookUI == null || parentTransform == null)
        {
            Debug.LogError("CookUI atau parentTransform tidak diisi!");
            return;
        }

        // Loop melalui semua resep di cookUI
        foreach (var recipe in cookUI.recipes)
        {
            // Buat GameObject baru untuk pembungkus hasil resep
            GameObject wrapper = new GameObject("Wrapper_" + recipe.result.itemName);
            wrapper.transform.SetParent(parentTransform, false); // Atur parent untuk wrapper
            wrapper.AddComponent<RectTransform>().sizeDelta = new Vector2(120, 120); // Ukuran pembungkus

            // Tambahkan komponen Image untuk pembungkus dan atur warnanya
            Image wrapperImage = wrapper.AddComponent<Image>();
            wrapperImage.color = backgroundColor;

            // Buat GameObject baru untuk hasil resep
            GameObject resultItem = new GameObject(recipe.result.itemName);
            resultItem.transform.SetParent(wrapper.transform, false); // Atur parent ke wrapper

            // Tambahkan komponen Image dan atur sprite-nya
            Image imageComponent = resultItem.AddComponent<Image>();
            imageComponent.sprite = recipe.result.sprite;

            // Tambahkan komponen Button
            Button buttonComponent = resultItem.AddComponent<Button>();
            buttonComponent.onClick.AddListener(() => DisplayRecipeInHasilCook(recipe, 0.8f));

            // Atur ukuran dan posisi resultItem agar berada di tengah wrapper
            RectTransform rectTransform = resultItem.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 100);  // Ukuran resultItem
            rectTransform.localPosition = Vector3.zero;      // Posisi di tengah wrapper
            rectTransform.localScale = Vector3.one;          // Pastikan skala sesuai
        }
    }

    public void DisplayRecipeInHasilCook(CookUI.CookRecipe recipe, float opacity)
    {
        // Pastikan HasilCook telah diinisialisasi sebelumnya
        if (hasilCook == null)
        {
            Debug.LogError("HasilCook belum diinisialisasi!");
            return;
        }

        DestroyRecipeInHasilCook();

        // Tambahkan komponen Image baru untuk menampilkan resep yang dipilih
        GameObject resultItem = new GameObject(recipe.result.itemName);
        resultItem.transform.SetParent(hasilCook.transform, false); // Atur parent ke HasilCook

        // Tambahkan komponen Image dan atur sprite-nya
        Image imageComponent = resultItem.AddComponent<Image>();
        imageComponent.sprite = recipe.result.sprite;

        // Tambahkan button untuk menghapus konten
        Button button = resultItem.AddComponent<Button>();
        button.onClick.AddListener(DestroyRecipeInHasilCook);

        // Atur opasitas gambar dengan mengubah nilai alpha warnanya
        Color imageColor = imageComponent.color;
        imageColor.a = opacity; // Ubah opasitas sesuai dengan nilai yang diberikan
        imageComponent.color = imageColor;

        // Atur ukuran dan posisi resultItem agar berada di tengah HasilCook
        RectTransform rectTransform = resultItem.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100);  // Ukuran resultItem
        rectTransform.localPosition = Vector3.zero;      // Posisi di tengah HasilCook
        rectTransform.localScale = Vector3.one;          // Pastikan skala sesuai

        CekIngredient1(recipe, opacity);
        CekIngredient2(recipe, opacity);
        CekIngredient3(recipe, opacity);
    }

    public void DestroyRecipeInHasilCook()
    {
        // Hapus semua child GameObject yang memiliki komponen Image di hasilCook, slotCook1, slotCook2, dan slotCook3
        ClearChildren(hasilCook);
        ClearChildren(slotCook1);
        ClearChildren(slotCook2);
        ClearChildren(slotCook3);
    }

    private void ClearChildren(GameObject parent)
    {
        for (int i = parent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                Destroy(child);
            }
        }
    }

    void CekIngredient1(CookUI.CookRecipe recipe, float opacity)
    {
        if (recipe.ingredients.Count > 0 && recipe.ingredients[0] != null)
        {
            GameObject resultItemIngredients1 = new GameObject(recipe.ingredients[0].itemName);
            resultItemIngredients1.transform.SetParent(slotCook1.transform, false);

            Image imageComponentIngredients1 = resultItemIngredients1.AddComponent<Image>();
            imageComponentIngredients1.sprite = recipe.ingredients[0].sprite;

            Color imageColorIngredients1 = imageComponentIngredients1.color;
            imageColorIngredients1.a = opacity;
            imageComponentIngredients1.color = imageColorIngredients1;

            RectTransform rectTransformIngredients1 = resultItemIngredients1.GetComponent<RectTransform>();
            rectTransformIngredients1.sizeDelta = new Vector2(100, 100);
            rectTransformIngredients1.localPosition = Vector3.zero;
            rectTransformIngredients1.localScale = Vector3.one;
        }
        else
        {
            Debug.Log("Resep ke 1 kosong");
        }
    }

    void CekIngredient2(CookUI.CookRecipe recipe, float opacity)
    {
        if (recipe.ingredients.Count > 1 && recipe.ingredients[1] != null)
        {
            GameObject resultItemIngredients2 = new GameObject(recipe.ingredients[1].itemName);
            resultItemIngredients2.transform.SetParent(slotCook2.transform, false);

            Image imageComponentIngredients2 = resultItemIngredients2.AddComponent<Image>();
            imageComponentIngredients2.sprite = recipe.ingredients[1].sprite;

            Color imageColorIngredients2 = imageComponentIngredients2.color;
            imageColorIngredients2.a = opacity;
            imageComponentIngredients2.color = imageColorIngredients2;

            RectTransform rectTransformIngredients2 = resultItemIngredients2.GetComponent<RectTransform>();
            rectTransformIngredients2.sizeDelta = new Vector2(100, 100);
            rectTransformIngredients2.localPosition = Vector3.zero;
            rectTransformIngredients2.localScale = Vector3.one;
        }
        else
        {
            Debug.Log("Resep ke 2 kosong");
        }
    }

    void CekIngredient3(CookUI.CookRecipe recipe, float opacity)
    {
        if (recipe.ingredients.Count > 2 && recipe.ingredients[2] != null)
        {
            GameObject resultItemIngredients3 = new GameObject(recipe.ingredients[2].itemName);
            resultItemIngredients3.transform.SetParent(slotCook3.transform, false);

            Image imageComponentIngredients3 = resultItemIngredients3.AddComponent<Image>();
            imageComponentIngredients3.sprite = recipe.ingredients[2].sprite;

            Color imageColorIngredients3 = imageComponentIngredients3.color;
            imageColorIngredients3.a = opacity;
            imageComponentIngredients3.color = imageColorIngredients3;

            RectTransform rectTransformIngredients3 = resultItemIngredients3.GetComponent<RectTransform>();
            rectTransformIngredients3.sizeDelta = new Vector2(100, 100);
            rectTransformIngredients3.localPosition = Vector3.zero;
            rectTransformIngredients3.localScale = Vector3.one;
        }
        else
        {
            Debug.Log("Resep ke 3 kosong");
        }
    }
}
