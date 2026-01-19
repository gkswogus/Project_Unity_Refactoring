=== killGostMonsterQuestFinish ===
{ KillGostMonsterQuestState :
    - "IN_PROGRESS" : -> inProgress
    - "CAN_FINISH" : -> canFinish
    - "FINISHED" : -> finished   
    - else : -> END
}

= inProgress
아직 몬스터를 다 잡지 못했는걸?
Q 키를 눌러서 퀘스트 현황을 확인해봐
-> END

= canFinish
수고 많았어!
여기 소소하지만 보상을 줄게!
그럼 잘가!
~OpenQuestRewardUI()
-> END

= finished
다음에도 도와줄거지?
-> END


