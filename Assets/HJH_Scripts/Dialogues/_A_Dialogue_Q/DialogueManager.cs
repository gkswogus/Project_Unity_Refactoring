using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("InkStory")]
    [SerializeField] private TextAsset inkJson;
    private Story story;
    private bool dialoguePlaying = false;

    private void Awake()
    {
        story = new Story(inkJson.text);
    }
    private void OnEnable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        GameEventManager.instance.inputEvents.interactionEvents.onNextDialogue += NextDialogue;
    }
    private void OnDisable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        GameEventManager.instance.inputEvents.interactionEvents.onNextDialogue -= NextDialogue;
    }
    private void NextDialogue()
    {
        if (!dialoguePlaying) return;
        ContinueOrExitStory();
    }
    private void EnterDialogue(string knotName)
    {
        if (dialoguePlaying) return;
        dialoguePlaying = true;

        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName); 
        }
        ContinueOrExitStory();
    }
    void ContinueOrExitStory()
    {
        if (story.canContinue)
        {
            string dialogueLine = story.Continue();

            Debug.Log(dialogueLine);
        }
        else
        {
            ExitDialogue();
        }
    }
    void ExitDialogue()
    {
        Debug.Log("Exit Dialogue");

        dialoguePlaying = false;
        story.ResetState();
    }
}
