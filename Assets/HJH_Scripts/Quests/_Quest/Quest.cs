using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Quest 
{
    public QuestInfo info; // 퀘스트 정보
    public QuestState state; // 퀘스트 진행 상태
    public QuestStep step; // 퀘스트 완료 조건

    public Quest(QuestInfo questInfo)
    { 
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;      
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform) // 퀘스트 스텝 프리펩 생성
    {
        if (info.questStepPrefab != null)
        {
            step = Object.Instantiate(info.questStepPrefab, parentTransform).GetComponent<QuestStep>();
            step.InitializeQuestStep(info.id);
        }
    }
}
 