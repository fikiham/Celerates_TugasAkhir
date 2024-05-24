using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance;

    [SerializeField] GameObject playerNameInputUI;




    public bool DoneFirstNarration;
    public bool DoneDialogue_1;
    public bool DoneDialogue_2;
    public bool DoneDialogue_3;
    public bool DoneDialogue_4;
    public bool DoneDialogue_5;
    public bool DoneDialogue_6;
    public bool DoneDialogue_7;

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
            Player_Direction.Instance.Target = ForestController.Instance.FirstSpawner.transform;
            Player_Quest.Instance.SetQuest("Kill all wolves");
        }
        else if (prompt == "1Dialogue")
        {
            DoneDialogue_1 = true;
            Player_Direction.Instance.Target = ForestController.Instance.DanauKetenangan;
            Player_Quest.Instance.SetQuest("Pergi ke danau ketenangan");
        }
        else if (prompt == "2Dialogue")
        {
            DoneDialogue_2 = true;
            Player_Direction.Instance.Target = ForestController.Instance.VillagePortal;
            Player_Quest.Instance.SetQuest("Kembali ke desa");
        }
        else if (prompt == "3Dialogue")
        {
            DoneDialogue_3 = true;
        }
        else if (prompt == "4Dialogue")
        {
            DoneDialogue_4 = true;
        }
        else if (prompt == "givePedang")
        {
            Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem("Pedang Ren"));
            ItemDisplay.Instance.ShowItemDisplay(ItemPool.Instance.GetItem("Pedang Ren"));
        }
        else if (prompt == "5Dialogue")
        {
            DoneDialogue_5 = true;
            Player_Quest.Instance.SetQuest("Bunuh semua bandit");
        }
        else if (prompt == "6Dialogue")
        {
            DoneDialogue_6 = true;
        }
        else if (prompt == "7Dialogue")
        {
            DoneDialogue_7 = true;
        }
        else
        {
            print("no valid prompt");
        }
    }
}
