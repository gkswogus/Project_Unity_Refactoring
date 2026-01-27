using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Properties;

public class QuestPoint : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private TextAsset inkJson;
    // [SerializeField] private string dialogueKnotName;

    [Header("Quest")] // 퀘스트 넣기
    [SerializeField] private QuestInfo questInfoForPoint;

    public QuestState currentQuestState;
    [Header("QuestPoint")] // 시작지점 or 완료지점 설정
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private string questId;
    private QuestIcon questIcon;

    private bool playerIsNear;

    private void Awake()
    {
        questIcon = GetComponentInChildren<QuestIcon>();
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventManager.instance.inputEvents.interactionEvents.onSubmitPressed += SubmitPressed;
    }
    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventManager.instance.inputEvents.interactionEvents.onSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed() // 퀘스트 시작을 위한 상호작용
    {
        if (!playerIsNear) return;

        if (inkJson != null)
        {
            GameEventManager.instance.dialogueEvents.EnterDialogue(inkJson, questInfoForPoint, currentQuestState);
        }
    }

    private void QuestStateChange(Quest quest) // 퀘스트 상태 아이콘 변경 이벤트
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) playerIsNear = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerIsNear = false;
    }

}