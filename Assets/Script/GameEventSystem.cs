using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance;

    [SerializeField] GameObject playerNameInputUI;


    public bool DoneFirstNarration;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void DoAfterDialogue(string prompt)
    {
        if (prompt == "playerName")
        {
            playerNameInputUI.SetActive(true);
        }
        else if (prompt == "firstNarration")
        {
            DoneFirstNarration = true;
        }
        else
        {
            print("no valid prompt");
        }
    }
}
