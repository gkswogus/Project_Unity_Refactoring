using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstQuestStep : QuestStep 
{
    private int KillMonster = 0;   
    private int CompleteKillMonster = 3; // 완료 몬스터 처치 수 

    private void OnEnable()
    {
       GameEventManager.instance.miscEvents.onKillMonstered += KillMonstered;
    }
    private void OnDisable()
    {  
        GameEventManager.instance.miscEvents.onKillMonstered -= KillMonstered;
    }

    private void KillMonstered()
    {
        if(KillMonster < CompleteKillMonster)
        {
            KillMonster++;
        }

        if (KillMonster >= CompleteKillMonster)
        {
            FinishQuestStep();
        }
    }
    public override string GetProgressText()
    {
        if (KillMonster == CompleteKillMonster)
        {
            return string.Format("악마 <color=blue>{0} / {1}</color>", KillMonster, CompleteKillMonster);
        }
        else
        {
            return string.Format("악마 {0}/{1}", KillMonster, CompleteKillMonster);
        }
    }
    public override string SaveStepData()
    {
        return KillMonster.ToString(); 
    }
    
    public override void LoadStepData(string state)
    {
        KillMonster = int.Parse(state); 
    }

 
}
