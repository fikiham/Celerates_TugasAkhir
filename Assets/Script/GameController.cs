using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] GameObject[] persistentUI;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPersistentUI(bool show)
    {
        foreach(GameObject ui in persistentUI)
        {
            ui.SetActive(show);
        }
    }


}
