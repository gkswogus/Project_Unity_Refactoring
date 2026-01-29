using System;

public class MiscEvents //몬스터 처치 퀘스트 이벤트
{
    public event Action onKillMonstered; //일반몬스터 처치 퀘스트 이벤트

    public void KillMonstered()
    {
        if (onKillMonstered != null)
        {
            onKillMonstered();
        }
    }

    public event Action onKillMonsteredMN;//엘리트 몬스터 처치 퀘스트 이벤트

    public void KillMonsterdMN()
    {
        if (onKillMonsteredMN != null)
        {
            onKillMonsteredMN();
        }
    }

    // 공통 몬스터 처치 퀘스트 이벤트
    public event Action<string> onKilledMonster;
    public void KilledMonster(string id)
    {
        onKilledMonster?.Invoke(id);
    }
}