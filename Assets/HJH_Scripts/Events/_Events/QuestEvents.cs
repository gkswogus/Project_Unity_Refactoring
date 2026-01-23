using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Profiling.Memory.Experimental;
public class QuestEvents
{
    public event Action<string> onStartQuest;

    public void StartQuest(string id)
    {
      onStartQuest?.Invoke(id);
    }

    public event Action<string> onAdvanceQuest;

    public void AdvanceQuest(string id)
    {
      onAdvanceQuest?.Invoke(id);
    }

    public event Action<string> onFinishQuest;

    public void FinishQuest(string id)
    {
      onFinishQuest?.Invoke(id);
    }

    public event Action<Quest> onQuestStateChange;

    public void QuestStateChange(Quest quest)
    {
       onQuestStateChange?.Invoke(quest);
    }

    // Äù½ºÆ® UI °ü·Ã //
    public event Action<Quest> onQuest_inp;
    public void Quest_inf(Quest quests)
    {
        onQuest_inp?.Invoke(quests);    
    }

    public event Action<Quest> onQuest_fin;
    public void Quest_fin(Quest quests)
    {
        onQuest_fin?.Invoke(quests);
    }

    public event Action<string> onQuestRewardInfo;
    public void QuestRewardInfo(string id)
    {
        onQuestRewardInfo?.Invoke(id);
    }

    public event Action<Quest> onQuestRewardUI;
    public void QuestRewardUI(Quest quest)
    {
        onQuestRewardUI?.Invoke(quest);
    }
    /*  public event Action<TMP_Text, TMP_Text, TMP_Text, TMP_Text> onProgressQuestUI;
      public void ProgressQuestUI(TMP_Text id,TMP_Text name,TMP_Text progress, TMP_Text reward)
      {
          onProgressQuestUI?.Invoke(id,name,progress,reward);
      }

      public event Action<TMP_Text> onFinishQuestUI;
      public void FinishQuestUI(TMP_Text qeuestid)
      {
          onFinishQuestUI?.Invoke(qeuestid);
      }

      public event Action<string> onRewardQuestUI;
      public void RewardQuestUI(string id)
      {
          onRewardQuestUI?.Invoke(id);
      }*/

    /*  public event Action<String, TMP_Text, TMP_Text> onRewardQuestUI;
      public void RewardQuestUI(String id, TMP_Text goldreward, TMP_Text Expreward)
      {
          onRewardQuestUI?.Invoke(id, goldreward, Expreward);
      }
  */
}
