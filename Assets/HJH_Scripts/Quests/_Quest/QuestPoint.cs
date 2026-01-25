using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    Transform player;
    float distance;
    float angleView;
    Vector3 direction;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

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
        if (!NearView()) return;

        if (inkJson != null &&
       (currentQuestState == QuestState.CAN_START ||
        currentQuestState == QuestState.IN_PROGRESS ||
        currentQuestState == QuestState.CAN_FINISH))
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

    bool NearView() // 시야 체크
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position); // 거리
        direction = this.transform.position - player.transform.position; //방향 벡터
        angleView = Vector3.Angle(player.transform.forward, direction);  // 바라보는 방향 각도
        if (angleView < 45f && distance < 5f)
            return true;
        else return false;
    }

}