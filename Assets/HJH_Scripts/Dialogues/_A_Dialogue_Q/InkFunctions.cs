using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
public class InkFunctions 
{
    // 잉크 시스템 C# 바인딩
    public void Bind(Story story)
    {
        story.BindExternalFunction("StartQuest", (string questid) => StartQuest(questid));
        story.BindExternalFunction("AdvanceQuest", (string questid) => AdvanceQuest(questid));
        story.BindExternalFunction("FinishQuest", (string questid) => FinishQuest(questid));
        story.BindExternalFunction("OpenQuestRewardUI", () =>
        {
            if (DialogueContext.CurrentQuestPoint != null)
            {
                DialogueContext.CurrentQuestPoint.OpenQuestRewardUI();
            }
        });
    }
    // 바인딩 해제
    public void UnBind(Story story)
    {
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("FinishQuest");
    }

    private void StartQuest(string questId)
    {
        GameEventManager.instance.questEvents.StartQuest(questId);
    }

    private void AdvanceQuest(string questId)
    {
        GameEventManager.instance.questEvents.AdvanceQuest(questId);
    }

    private void FinishQuest(string questId)
    {
        GameEventManager.instance.questEvents.FinishQuest(questId);
    }

}

