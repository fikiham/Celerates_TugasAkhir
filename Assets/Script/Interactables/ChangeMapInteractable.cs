using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMapInteractable : Interactable
{
    [SerializeField] bool goToVillage;

    protected override void Interact()
    {
        string direction = goToVillage ? "Village" : "Forest";
        Debug.Log("Going to " + direction);
        GameController.Instance.SaveGame();
        LoadingScreenUI.Instance.LoadScene(goToVillage ? 2 : 1);
        //SceneManager.LoadScene(goToVillage ? 2 : 1);
        GameController.Instance.LoadGame();

        // Jika pindah ke scene "Forest", mainkan suara transisi hutan
        if (!goToVillage)
        {
            Debug.Log("Forest di jalankan");
            SoundManager.Instance.PlaySound("Forest"); // Pastikan "Forest" terdaftar di SoundManager
        }
    }
}
