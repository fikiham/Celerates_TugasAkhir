using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Dialogues theDialogues;
    Queue<Dialogues.Dialogue> dialogues = new();

    [SerializeField] GameObject dialogueUI;
    [SerializeField] TMP_Text[] speakersText;
    string firstSpeaker;
    [SerializeField] TMP_Text dialogueText;

    [SerializeField] Button NextButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartFirstDialogue();
        }
    }

    public void StartFirstDialogue()
    {
        StartDialogue(theDialogues);
    }
    public void StartDialogue(Dialogues theDialogues)
    {
        dialogueUI.SetActive(true);
        firstSpeaker = null;
        dialogues = new();
        foreach (Dialogues.Dialogue dialogue in theDialogues.TheDialogues)
        {
            if (firstSpeaker == null)
                firstSpeaker = dialogue.name;
            dialogues.Enqueue(dialogue);
        }

        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(NextDialogue);

        NextDialogue();
    }

    public void NextDialogue()
    {
        if (dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogues.Dialogue dialogue = dialogues.Dequeue();
        if (dialogue.name == firstSpeaker)
        {
            speakersText[0].text = dialogue.name;
            speakersText[0].gameObject.SetActive(true);
            speakersText[1].gameObject.SetActive(false);
        }
        else
        {
            speakersText[1].text = dialogue.name;
            speakersText[0].gameObject.SetActive(false);
            speakersText[1].gameObject.SetActive(true);
        }
        SetText(dialogueText, dialogue.sentence);
    }

    public void EndDialogue()
    {
        print("End of conversations");
        dialogueUI.SetActive(false);
    }


    void SetText(TMP_Text text, string value, float dur = 1)
    {
        StartCoroutine(SettingText(text, value, dur));
    }
    IEnumerator SettingText(TMP_Text text, string value, float dur = 1)
    {
        float waitTime = dur / (value.Length * 3);
        float startTime = Time.time;

        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(StopAllCoroutines);
        NextButton.onClick.AddListener(() => text.text = value);
        NextButton.onClick.AddListener(() => NextButton.onClick.RemoveAllListeners());
        NextButton.onClick.AddListener(() => NextButton.onClick.AddListener(NextDialogue));
        for (int i = 0; i < value.Length; i++)
        {
            text.text = value[..(i + 1)];
            yield return new WaitForSeconds(waitTime);
        }
        print(Time.time - startTime);
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(NextDialogue);

    }
}
