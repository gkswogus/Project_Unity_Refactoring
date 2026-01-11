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
    private void NextDialogue() // G키 입력을 통해 다음 대사로 넘기기
    {
        if (!dialoguePlaying) return;
        ContinueOrExitStory();
    }
    private void EnterDialogue(string knotName) // 대화 시작
    {
        if (dialoguePlaying) return;

        dialoguePlaying = true;

        if (!knotName.Equals(""))

        GameEventManager.instance.dialogueEvents.DialogueStarted();

        {
            story.ChoosePathString(knotName); 
        }
        ContinueOrExitStory();
    }

    void ContinueOrExitStory() // 대화 진행 중
    {
        if (story.canContinue)
        {
            string dialogueLine = story.Continue();
            GameEventManager.instance.dialogueEvents.DialougeDisplay(dialogueLine);
        }
        else
        {
            ExitDialogue();
        }
    }

    void ExitDialogue() // 대화 종료
    {
        dialoguePlaying = false;

        GameEventManager.instance.dialogueEvents.DialogueFinished();

        story.ResetState();
    }
}
