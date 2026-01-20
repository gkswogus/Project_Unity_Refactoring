EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL OpenQuestRewardUI()

//VAR KillGostMonsterQuestId = "KillGostMonster"
VAR KillGostMonsterQuestState ="REQUIREMENTS_NOT_MET"


INCLUDE killGostMonsterQuestStartNpc.ink
INCLUDE killGostMonsterQuestFinishNpc.ink