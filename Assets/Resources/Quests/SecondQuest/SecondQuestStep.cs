using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondQuestStep : QuestStep
{
    private int KillMonsterMN = 0;     //초기 값 0Misc
    private int CompleteKillMonsterMN = 1; // 완료 몬스터 처치 수 

    private void OnEnable()
    {
        GameEventManager.instance.miscEvents.onKillMonsteredMN += KillMonsteredMN;
    }
    private void OnDisable()
    {
        GameEventManager.instance.miscEvents.onKillMonsteredMN -= KillMonsteredMN;
    }

    private void KillMonsteredMN()
    {
        if (KillMonsterMN < CompleteKillMonsterMN)
        {
            KillMonsterMN++;
        }

        if (KillMonsterMN >= CompleteKillMonsterMN)
        {
            FinishQuestStep();     
        }
    }
    public override string GetProgressText()
    {
        if (KillMonsterMN == CompleteKillMonsterMN)
        {
            return string.Format("미노타우르스 <color=blue>{0} / {1}</color>", KillMonsterMN, CompleteKillMonsterMN);
        }
        else
        {
            return string.Format("미노타우르스 {0}/{1}", KillMonsterMN, CompleteKillMonsterMN);
        }
    }
    public override string SaveStepData()
    {
        return KillMonsterMN.ToString(); 
    }
    
    public override void LoadStepData(string state)
    {
        KillMonsterMN = int.Parse(state);
    }
}
