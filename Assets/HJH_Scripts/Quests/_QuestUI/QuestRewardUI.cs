using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestRewardUI : MonoBehaviour
{
    [Header("퀘스트 보상 창")]
    [SerializeField] GameObject reWardUI;
    [SerializeField] private TMP_Text goldRewardText;
    [SerializeField] private TMP_Text expRewardText;
    [SerializeField] private UnityEngine.UI.Button btn;

    private string questId; // 서버(퀘스트매니저) 쪽으로 보낼 퀘스트 정보
    void Start()
    {
        reWardUI.SetActive(false);
    }
    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onQuestRewardUI += QuestRewardUiOpen;
    }
    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onQuestRewardUI -= QuestRewardUiOpen;
    }
    private void QuestRewardUiOpen(Quest quest)
    {
        this.questId = quest.info.id;
        goldRewardText.text = quest.info.goldReward.ToString();
        expRewardText.text = quest.info.expReward.ToString();
        reWardUI.SetActive(true);
        UIEventControll.instance.isOnUI = true; // 마우스 제어
    }

    public void OnClickConfirm() //보상 받기 버튼 클릭 시
                                 //UI 끄기 및 퀘스트 완료 + 보상 획득 요청
    {
        GameEventManager.instance.questEvents.FinishQuest(questId);
        SoundManager.instance.Play(UISOUND.QuestC);
        reWardUI.SetActive(false);
        UIEventControll.instance.isOnUI = false;
    }

}
