using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Rendering;
using JetBrains.Annotations;

public class DialogueManager : MonoBehaviour
{
 //   [Header("InkStory")]
  //  [SerializeField] private TextAsset inkJson;
    private Story story;
    private bool dialoguePlaying = false;
    private int currentChoiceIndex = -1;

    private InkFunctions inkFunctions;


    private void Awake()
    {
          inkFunctions = new InkFunctions();
    }
 
    private void OnEnable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        GameEventManager.instance.dialogueEvents.onChoiceIndex += ChoiceIndex;
        GameEventManager.instance.inputEvents.interactionEvents.onNextDialogue += NextDialogue;
        GameEventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    private void OnDisable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        GameEventManager.instance.dialogueEvents.onChoiceIndex -= ChoiceIndex;
        GameEventManager.instance.inputEvents.interactionEvents.onNextDialogue -= NextDialogue;
        GameEventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

   

    private void QuestStateChange(Quest quest)
    {
        GameEventManager.instance.dialogueEvents.UpdateInkDialogueVariable(
            quest.info.id + "QuestState",
            new StringValue(quest.state.ToString())
            );
    }

    private void ChoiceIndex(int index)
    {
        currentChoiceIndex = index;
    }
    private void NextDialogue() // G키 입력을 통해 다음 대사로 넘기기
    {
        if (!dialoguePlaying) return;
        ContinueOrExitStory();
    }
    private void EnterDialogue(TextAsset npcInkJson, QuestInfo info,QuestState state) // 대화 시작
    {
        if (dialoguePlaying) return;

        story = new Story(npcInkJson.text);
        inkFunctions.Bind(story);

        dialoguePlaying = true;
        GameEventManager.instance.dialogueEvents.DialogueStarted(); // 대화UI 오브젝트 켜기
        story.variablesState.SetGlobal(info.id + "QuestState", new StringValue(state.ToString()));

        ContinueOrExitStory();
    }

    void ContinueOrExitStory() // 대화 진행 중
    {
        if(story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = -1; // 다음 선택지로 값이 넘어가는것을 방지
        } 

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();
            
            // 줄이 비어있어도, 대화가 계속 진행될 수 있다면 줄바꿈 진행
            while (LineFix(dialogueLine) && story.canContinue)
            {
                dialogueLine = story.Continue();
            } 

            if(LineFix(dialogueLine) && !story.canContinue)
            {
                ExitDialogue();
            }
            else
            {
                GameEventManager.instance.dialogueEvents.DialougeDisplay(dialogueLine, story.currentChoices);
            } 
        }
        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }
    }

    void ExitDialogue() // 대화 종료
    {
        dialoguePlaying = false;

        GameEventManager.instance.dialogueEvents.DialogueFinished();

        if (story != null)
        {
            story = null;
        }
        DialogueContext.CurrentQuestPoint = null;
    }

    private bool LineFix(string dialogueLine) // 잉크 문제. 대화 선택지 선택 시, 빈 텍스트 출력 삭제
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }
}
