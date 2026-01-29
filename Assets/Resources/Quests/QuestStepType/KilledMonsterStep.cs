using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledMonsterStep : QuestStep
{
    [SerializeField] private ParentState targetMonsterInfo;
    [SerializeField] private int requiredKillCount;

    private int currentKillCount = 0;     
    private void OnEnable()
    {
        GameEventManager.instance.miscEvents.onKilledMonster += KilledMonster;
    }
    private void OnDisable()
    {
        GameEventManager.instance.miscEvents.onKilledMonster -= KilledMonster;
    }

    private void KilledMonster(string id)
    {
        if (id != targetMonsterInfo.p_name) return;

        if (currentKillCount < requiredKillCount)
        {
            currentKillCount++;
        }

        if (currentKillCount >= requiredKillCount)
        {
            FinishQuestStep();
        }
    }
    public override string GetProgressText()
    {
        if (currentKillCount == requiredKillCount)
        {
            return string.Format(targetMonsterInfo.p_name + " <color=blue>{0} / {1}</color>", currentKillCount, requiredKillCount);
        }
        else
        {
            return string.Format(targetMonsterInfo.p_name + " {0}/{1}", currentKillCount, requiredKillCount);
        }
    }
    public override string SaveStepData()
    {
        return currentKillCount.ToString();
    }

    public override void LoadStepData(string state)
    {
        currentKillCount = int.Parse(state);
    }
}
