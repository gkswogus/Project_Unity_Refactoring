using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public List<SavedQuestData> quests;  // 퀘스트들의 저장 정보

    [System.Serializable]
    public class SavedQuestData
    {
        public string questId;  // 퀘스트 ID
        public QuestState questState;  // 퀘스트 상태

        public string stepState; // 현재는 정수값(int)만 있지만, 나중에 "~와 대화하기 완료" 같은 string값도 있을 수 있으니 
                                 // string으로선언하고, int형으로 변환
    }
}
