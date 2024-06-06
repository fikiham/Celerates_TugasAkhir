using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KakRenInteractable : Interactable
{
    [SerializeField] Dialogues SecDialogue;
    protected override void Interact()
    {
        if (GameEventSystem.Instance.DoneDialogue_FirstDesaWarga && !GameEventSystem.Instance.DoneDialogue_FirstKakRen)
        {
            Debug.Log("cerita kak ren jalan ");
            DialogueSystem.Instance.StartDialogue(SecDialogue);
        }
    }
}
