using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDialogueInteractable : Interactable
{

    [SerializeField] Dialogues firDialogue;
    protected override void Interact()
    {
        if (!GameEventSystem.Instance.DoneDialogue_1)
            DialogueSystem.Instance.StartDialogue(firDialogue);
    }
}
