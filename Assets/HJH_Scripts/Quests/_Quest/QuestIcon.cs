using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject canNotStartIcon; // 시작 불가
    [SerializeField] private GameObject canStartIcon; // 시작 가능
    [SerializeField] private GameObject canNotFinishIcon; // 진행 중
    [SerializeField] private GameObject canFinishIcon; // 완료 가능

    public void SetState(QuestState newState, bool startPoint, bool finishPoint)
    {
        canNotStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        canNotFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        switch (newState)
        {
            case QuestState.REQUIREMENTS_NOT_MET: // 퀘스트 시작 불가 상태
                if (startPoint) { canNotStartIcon.SetActive(true); }
                break;
            case QuestState.CAN_START: // 퀘스트 시작 가능 상태
                if (startPoint) { canStartIcon.SetActive(true); }
                break; 
            case QuestState.IN_PROGRESS: // 퀘스트 진행 중 상태
                if (finishPoint) { canNotFinishIcon.SetActive(true); }
                break;
            case QuestState.CAN_FINISH: // 퀘스트 완료 가능 상태
                if (finishPoint) { canFinishIcon.SetActive(true); }
                break;
            case QuestState.FINISHED:
                break;
        }
    }
}
