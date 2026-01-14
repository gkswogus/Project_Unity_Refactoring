using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueEvents : MonoBehaviour
{
    public event Action<string> onEnterDialogue;
    public void EnterDialogue(string dialogue)
    {
        onEnterDialogue?.Invoke(dialogue);
    }

    public event Action onDialogueStarted;
    public void DialogueStarted()
    {
        onDialogueStarted?.Invoke();
    }

    public event Action onDialogueFinished;
    public void DialogueFinished()
    {
        onDialogueFinished?.Invoke();
    }

    public event Action<string,List<Choice>> onDialogueDisplay;
    public void DialougeDisplay(string display,List<Choice> dialogueChoices) // 선택지 정보 전달 추가
    {
        onDialogueDisplay?.Invoke(display, dialogueChoices);
    }

    public event Action<int> onChoiceIndex;
    public void ChoiceIndex(int index)
    {
        onChoiceIndex?.Invoke(index);
    }
}
