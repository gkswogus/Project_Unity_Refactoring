using Ink.Parsed;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;

    public Coroutine typingCoroutine;
    private string fullDialogueText;

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

    private void DialogueDisplay(string display, List<Ink.Runtime.Choice> dialogueChoices)
    {
        fullDialogueText = display;
        typingCoroutine = StartCoroutine(TypeTextCoroutine(display));

        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false); 
        }

        int ChoiceButtonIndex = dialogueChoices.Count - 1; // 배열 인덱스 
        // 잉크 파일에 선택지를 순회하는 루프
        for (int inkChoiceIndex =0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Ink.Runtime.Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtons[ChoiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if(inkChoiceIndex == 0) // 첫 번째 선택지가 선택 된 상태로 시작
            {
                choiceButton.SelectButton();
                GameEventManager.instance.dialogueEvents.ChoiceIndex(0);
            }

            ChoiceButtonIndex--; // 선택지는 거꾸로 노출하기 때문에 상위부터 마이너스하기
        }

    }
    private IEnumerator TypeTextCoroutine(string text)
    {
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f); // 한 글자 출력 간격 조절 가능
        }
        typingCoroutine = null;
    }
    public void SkipTyping() // 코루틴이 이미 돌고 있으면 멈추고 전체 텍스트 즉시 출력
    {     
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullDialogueText;
            typingCoroutine = null;
        }
    }
    private void ResetDialogueText()
    {
        dialogueText.text = "";
    }
}
