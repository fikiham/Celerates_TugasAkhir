using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance;

    [SerializeField] GameObject playerNameInputUI;

    private void Awake()
    {
        Instance = this;
    }

    public void DoAfterDialogue(string prompt)
    {
        if (prompt == "playerName")
        {
            playerNameInputUI.SetActive(true);
        }
        else
        {
            print("no valid prompt");
        }
    }
}
