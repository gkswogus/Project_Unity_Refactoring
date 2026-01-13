using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class DialogueChoiceButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text choiceText;

    private int choiceIndex = -1;

    public void SetChoiceIndex(int index)
    {
        choiceIndex = index;
    }

    public void SetChoiceText(string choiceTextSting)
    {
        choiceText.text = choiceTextSting;
    }

    public void SelectButton()
    {
        btn.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventManager.instance.dialogueEvents.ChoiceIndex(choiceIndex);
    }

  

   

   
}
