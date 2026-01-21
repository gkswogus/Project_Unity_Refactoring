=== killGostMonsterQuestStart ===
VAR KillGostMonsterQuestState ="REQUIREMENTS_NOT_MET"
{ KillGostMonsterQuestState :
   - "REQUIREMENTS_NOT_MET" : -> requirementsNotMet
   - "CAN_START" : -> canStart
   - "IN_PROGRESS" : -> inProgress
   - "CAN_FINISH" : -> canFinish
   - "FINISHED" : -> finished
   - else : -> END
}

= requirementsNotMet
음.. 나에게 무슨 볼일이라도 있어?
-> END   

= canStart
처음보는 친구인걸?
혹시 유령 몬스터를 처치하고 와줄 수 있어?
* [YES]
 ~StartQuest("KillGostMonster")
정말 고마워! 너라면 충분히 해낼 수 있을거야.
* [NO] 
그래? 마음이 바뀌면 언제든지 나에게 다시 찾아와줘!
- -> END  

= inProgress
아직 몬스터를 다 잡지 못했는걸? 
Q 키를 눌러서 퀘스트 현황을 확인해 봐
-> END

= canFinish
오! 몬스터를 다 잡았구나? 
옆에 있는 NPC에게 말을 걸어 봐!
-> END

= finished 
다음에도 부탁 들어줄꺼지? 
-> END