using UnityEngine;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour
{
    public void SubmitPressed(InputAction.CallbackContext context)// Y
    {
        if (context.performed)
            GameEventManager.instance.inputEvents.interactionEvents.SubmitPressed();
    }
    public void QuestWindowOn_Off(InputAction.CallbackContext context)
    {
        if (context.performed)
            GameEventManager.instance.inputEvents.uiEvent.QuestWindowOn_Off();
    }
    public void STDWindowOn_OFF(InputAction.CallbackContext context)
    {
        if(context.performed)
           GameEventManager.instance.inputEvents.uiEvent.STDWindowOn_OFF();
    }
    public void InventoryOn_OFF(InputAction.CallbackContext context)
    {
        if (context.performed)
            GameEventManager.instance.inputEvents.uiEvent.InventoryOn_OFF();
    }
    public void NextDialogue(InputAction.CallbackContext context) // G
    {
        if(context.performed)
            GameEventManager.instance.inputEvents.interactionEvents.NextDialogue();
    }
 
}