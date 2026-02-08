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
 
}
