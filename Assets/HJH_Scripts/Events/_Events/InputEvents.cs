using UnityEngine;
using System;

public class UIInputEvents
{
    public event Action OnQuestWindowOn_Off;
    public void QuestWindowOn_Off()
    { 
       OnQuestWindowOn_Off?.Invoke();
    }
    public event Action OnSTDWindowOn_OFF;
    public void STDWindowOn_OFF()
    {
       OnSTDWindowOn_OFF?.Invoke();
    }
    public event Action OnInventoryOn_OFF;
    public void InventoryOn_OFF()
    {
       OnInventoryOn_OFF?.Invoke();
    }
}
public class InteractionInputEvents
{
    public event Action onSubmitPressed;
    public void SubmitPressed()
    {
        onSubmitPressed?.Invoke();
    }
    public event Action onNextDialogue;
    public void NextDialogue()
    {
        onNextDialogue?.Invoke();
    }
      
}
public class InputEvents
{
    public UIInputEvents uiEvent;
    public InteractionInputEvents interactionEvents;
    public InputEvents()
    {
        uiEvent = new UIInputEvents();
        interactionEvents = new InteractionInputEvents();
    }  
}