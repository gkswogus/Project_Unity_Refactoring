using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void FinishQuest(string id)
    {
        // 실제 인벤토리 시스템 연동 시 아이템 지급 로직 추가
        Debug.Log("장비 아이템 지급 관련 내용");
    }

    public override string GetProgressText()
    {
        return string.Empty;
    }
    public override string SaveStepData()
    {
        return string.Empty;
    }

    public override void LoadStepData(string state)
    {
        
    }
}
