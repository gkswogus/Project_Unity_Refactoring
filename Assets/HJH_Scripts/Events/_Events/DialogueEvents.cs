using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public event Action<string> onDialogueDisplay;
    public void DialougeDisplay(string display)
    {
        onDialogueDisplay?.Invoke(display);
    }

    public event Action<int> onChoiceIndex;
    public void ChoiceIndex(int index)
    {
        onChoiceIndex?.Invoke(index);
    }
}
