using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Quest : MonoBehaviour
{
    public static Player_Quest Instance;

    private void Awake()
    {
        if (Instance = null)
            Instance = this;
    }

    public void SetQuest(string quest)
    {
        PlayerUI.Instance.currentQuestText.text = quest;
    }
}
