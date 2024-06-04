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
            DialogueSystem.Instance.StartDialogue(SecDialogue);
        }
    }
}
