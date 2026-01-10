using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public interface IQuestProgress
{
    string GetProgressText();
}*/

public abstract class QuestStep : MonoBehaviour //추상클래스 ( 오직  상속만을 위한 - abstract)
{
   // private bool isFinished = false; // 왜있지?

    public string questId { get; private set; }

    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;
    }
  
    protected void FinishQuestStep()
    {
        GameEventManager.instance.questEvents.AdvanceQuest(questId);
    }

    /* if (!isFinished)
     {
         isFinished = true;
         GameEventManager.instance.questEvents.AdvanceQuest(questId);
     }*/

    
    public abstract string SaveStepData(); // 퀘스트 진행 상황 저장
    public abstract void LoadStepData(string state); // 퀘스트 진행 상황 로드
    public abstract string GetProgressText(); // 퀘스트 진행 상황 정보 Text
}
