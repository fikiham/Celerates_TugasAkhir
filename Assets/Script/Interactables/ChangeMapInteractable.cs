using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMapInteractable : Interactable
{
    [SerializeField] bool goToVillage;

    protected override void Interact()
    {
        string direction = goToVillage ? "Village" : "Forest";
        Debug.Log("Going to " + direction);
        SceneManager.LoadScene(goToVillage ? 2 : 1);
    }

}
