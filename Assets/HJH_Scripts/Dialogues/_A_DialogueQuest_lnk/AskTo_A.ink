INCLUDE all_dailogue.ink

VAR AskTo_AQuestState =""
{ AskTo_AQuestState :
   - "CAN_START" : -> canStart
   - else : -> END
}

= canStart
상당히 초라해 보이시는군요
제가 당신이 마음에 들만한 아이템을 드리겠습니다
받아 보시겠습니까?
* [YES]
~ StartQuest("AskTo_A")
 ~ AdvanceQuest("AskTo_A")
그럼 탁자에 다가가서 "Y"키로 획득하세요
 ~FinishQuest("AskTo_A")
* [NO] 
다음에 다시 오시죠 그럼 
- -> END  



