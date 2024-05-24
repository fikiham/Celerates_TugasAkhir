using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedangRenInteractable : Interactable
{
    [SerializeField] Dialogues pedangRenDialogue;
    protected override void Interact()
    {
        if (GameEventSystem.Instance.DoneDialogue_4 && !GameEventSystem.Instance.DoneDialogue_5)
            DialogueSystem.Instance.StartDialogue(pedangRenDialogue);
    }
}
