using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
        ResetDialogueText();
    }

    private void OnEnable()
    {
        GameEventManager.instance.dialogueEvents.onDialogueStarted += DialogueStarted;
        GameEventManager.instance.dialogueEvents.onDialogueFinished += DialougeFinished;
        GameEventManager.instance.dialogueEvents.onDialogueDisplay += DialogueDisplay;
    }
    private void OnDisable()
    {
        GameEventManager.instance.dialogueEvents.onDialogueStarted -= DialogueStarted;
        GameEventManager.instance.dialogueEvents.onDialogueFinished -= DialougeFinished;
        GameEventManager.instance.dialogueEvents.onDialogueDisplay -= DialogueDisplay;
    }

    private void DialogueStarted()
    {
        dialoguePanel.SetActive(true);
    }

    private void DialougeFinished()
    {
        dialoguePanel.SetActive(false);

        ResetDialogueText();
    }

    private void DialogueDisplay(string display)
    {
        dialogueText.text = display;   
    }

    private void ResetDialogueText()
    {
        dialogueText.text = "";
    }
}
