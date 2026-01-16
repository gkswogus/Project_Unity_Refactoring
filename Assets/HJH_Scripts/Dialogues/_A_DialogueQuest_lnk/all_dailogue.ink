EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)

-> 유령소탕

=== 유령소탕 ===
처음보는 친구인걸?
혹시 유령 몬스터를 처치하고 와줄 수 있어?
* [YES]
 ~ StartQuest("악마 처치")
정말 고마워! 너 라면 충분히 해낼 수 있을거야.
* [NO] 
그래? 마음이 바뀌면 언제든지 나에게 다시 찾아와줘!
- -> END  
