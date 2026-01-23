INCLUDE all_dailogue.ink

VAR KillCowMonsterQuestState =""
{ KillCowMonsterQuestState :
   - "REQUIREMENTS_NOT_MET" : -> requirementsNotMet
   - "CAN_START" : -> canStart
   - "IN_PROGRESS" : -> inProgress
   - "CAN_FINISH" : -> canFinish
   - "FINISHED" : -> finished
   - else : -> END
}

= requirementsNotMet
여기서 어떻게 나가지...
-> END   

= canStart
기사님!
혹시 저 성문 밖에 황소 몬스터를  처치하고 와줄 수 있어요?
제가 저 무시무시한 몬스터 때문에 밖을 못나가고 있어요.. 부탁드려요.
* [YES]
 ~StartQuest("KillCowMonster")
감사합니다 기사님!
* [NO] 
다음에 꼭 도와주세요!
- -> END  

= inProgress
아직 몬스터가 밖에 있는거 같아요..
-> END

= canFinish
와! 정말 대단해요 기사님!
정말 강력하시군요!
여기 제 성의를 담은 보상을 받아주세요!
~OpenQuestRewardUI("KillCowMonster")
-> END

= finished 
정말 감사합니다. 
덕분에 마을 밖이 안전해 졌어요.
-> END
