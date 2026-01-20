using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestPoint : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;

    [Header("Quest")] // 퀘스트 넣기
    [SerializeField] private QuestInfo questInfoForPoint;
    public QuestInfo QuestInfoForPoint => questInfoForPoint; // 읽기 전용 프로퍼티
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

    private void SubmitPressed() // 퀘스트 시작 or 완료 이벤트
    {
        if (!NearView()) return;

        if (!dialogueKnotName.Equals(""))
        {
            DialogueContext.CurrentQuestPoint = this;
            GameEventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName, QuestInfoForPoint, currentQuestState);
        }
        else
        {
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventManager.instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                OpenQuestRewardUI();
            }
        }

    }
    public void OpenQuestRewardUI() // 퀘스트 완료 할 때 호출 => Ink 연동하기
    {
        Quest_UiManager.Instance.Open(this);
    }

    public void ReWardGet() // UiManager에서 버튼 클릭 이벤트 함수 연동 => 클릭 시 호출
    {
        GameEventManager.instance.questEvents.FinishQuest(questId);
        SoundManager.instance.Play(UISOUND.QuestC);
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