using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quest_UiManager : MonoBehaviour
{
    public static Quest_UiManager Instance { get; private set; }
    [Header("퀘스트 보상 창")]
    [SerializeField] GameObject reWardUI; 
    [SerializeField] private TMP_Text goldRewardText;
    [SerializeField] private TMP_Text expRewardText;

    [Header("퀘스트 정보 창")]
    [SerializeField] GameObject questUI;
    [SerializeField] TMP_Text questName;
    [SerializeField] TMP_Text questIsPlay;
    [SerializeField] TMP_Text questGoal;
    [SerializeField] TMP_Text questReward;
    [SerializeField] GameObject questidPrefab;
    [SerializeField] Transform inp_questListContent;
    [SerializeField] Transform fin_questListContent;
    [SerializeField] Button progressBTN;
    [SerializeField] Button finishBTN;

    private bool isQustUI;

    [Header("퀘스트 매니저")]
    [SerializeField] QuestManager qm;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        reWardUI.SetActive(false);
        questUI.SetActive(false);

        GameEventManager.instance.inputEvents.uiEvent.OnQuestWindowOn_Off += QuestWindowOn_Off;

        GameEventManager.instance.questEvents.onQuest_inp += Quest_Inprogress;
        GameEventManager.instance.questEvents.onQuest_fin += Quest_Finish;
    }
    //******************************************************************// 이하 보상 창
    /*  public void ShowRewardText(Quest quest) // 퀘스트 완료 시, 보상 창
      {
          goldRewardText.text = $"{quest.info.goldReward}";
          expRewardText.text = $"{quest.info.expReward}";
          reWardUI.SetActive(true);
      }*/
    
    public void ShowRewardText(QuestInfo quest) // 퀘스트 완료 시, 보상 창
    {
        goldRewardText.text = $"{quest.goldReward}";
        expRewardText.text = $"{quest.expReward}";
        reWardUI.SetActive(true);
    }
    public void ColseRewardUI()
    {
        reWardUI.SetActive(false);
    }
    //******************************************************************// 이하 정보 창
/*이전 구조: QuestUIManager가 QuestManager 리스트를 전체 참조 → foreach 돌면서 UI 생성

지금 구조: 퀘스트 시작 시 → 이벤트로 Quest 한 개 전달 → Quest_Inprogress(Quest quest)에서 처리

퀘스트 완료 시 → 이벤트로 Quest 한 개 전달 → Quest_Finish(Quest quest)에서 처리

장점: 전체 리스트를 매번 순회할 필요 없음 → 10,000개 퀘스트 있어도 효율적*/
//해야할거. 지금 UI가 Quest에 정보를 직접적으로 전달받고있다. 중간에 거치는 Ui전용 데이터 클래스 만들어서 받아오자.

    private void QuestWindowOn_Off() 
    {
        /*   List<Quest> inprogress_Qust = qm.getInprogress_Q;
           List<Quest> finish_Quest = qm.getFinish_Q;

           OpenQuestWindow(inprogress_Qust, finish_Quest);*/
        isQustUI = !isQustUI;
        questUI.SetActive(isQustUI);
        UIEventControll.instance.isOnUI = UIEventControll.instance.isOnUI ? false : true;
        ClearQuestDetail();
        ProgressBtn();

      //  ClearQuestDetail(); // finish삭제로 바꿔
      //   SetButtonColor(progressBTN);
      //  SetButtonColor(finishBTN);
    }
    /*    public void OpenQuestWindow(List<Quest> inprogress_Qust, List<Quest> finish_Quest)
        {
            isQustUI = !isQustUI;
            questUI.SetActive(isQustUI);
            UIEventControll.instance.isOnUI = UIEventControll.instance.isOnUI ? false : true;

            this.inprogress_finishQuest = inprogress_Qust;
            this.finishQuest = finish_Quest;
            ProgressBTN(); 
        }*/

    /* public void ProgressBTN() // 진행중인 퀘스트 버튼 클릭
     {
       //  ClearQuestList();
         ClearQuestDetail();
         SetButtonColor(progressBTN);

         foreach (Quest quest in inprogress_finishQuest)
         {
             GameObject go = Instantiate(questidPrefab, inp_questListContent);
             TMP_Text text = go.GetComponentInChildren<TMP_Text>();
             text.text = quest.info.id;

          //   Quest capturedQuest = quest; 
             go.GetComponent<Button>().onClick.AddListener(() => ShowQuestDetail(quest));
         }

         if (inprogress_finishQuest.Count == 0) questName.text = $"진행중인 퀘스트가 없습니다.";
     }*/
    public void ProgressBtn()
    {
        SetButtonColor(progressBTN);
        inp_questListContent.gameObject.SetActive(true);
        fin_questListContent.gameObject .SetActive(false);
        ClearQuestDetail();
    }
    public void FinishBtn()
    {
        SetButtonColor(finishBTN);
        inp_questListContent.gameObject.SetActive(false);
        fin_questListContent.gameObject.SetActive(true);
        ClearQuestDetail();
    }

    private Dictionary<string, GameObject> inProgressQuest = new Dictionary<string, GameObject>();
    void Quest_Inprogress(Quest quest)
    {

        if (inProgressQuest.ContainsKey(quest.info.id))
            return;

            GameObject go = Instantiate(questidPrefab, inp_questListContent);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            text.text = quest.info.id;

            go.GetComponent<Button>().onClick.AddListener(() => ShowQuestDetail(quest));

            inProgressQuest.Add(quest.info.id, go);      
    }

    private Dictionary<string, GameObject> finishedQuest = new Dictionary<string, GameObject>();
    public void Quest_Finish(Quest quest) // 완료한 퀘스트 버튼 클릭
    {
       
            //진행중인 퀘스트 넘어오면 삭제 
            if (inProgressQuest.TryGetValue(quest.info.id, out GameObject inGo))
            {
                Destroy(inGo);
                inProgressQuest.Remove(quest.info.id);
            }

        if (finishedQuest.ContainsKey(quest.info.id))
            return; 

        // 새 퀘스트만 생성
           GameObject go = Instantiate(questidPrefab, fin_questListContent);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            text.text = quest.info.id;

            go.GetComponent<Button>().onClick.AddListener(() => ShowFinishQuestDetail(quest));

              finishedQuest.Add(quest.info.id, go);
        
    }

/*    private void ClearQuestList()
    {
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }
    }*/
    private void ClearQuestDetail()
    {
        questName.text = "";
        questIsPlay.text = "";
        questGoal.text = "";
        questReward.text = "";
    }
    private void ShowQuestDetail(Quest quest)
    {
       questName.text = $"-{quest.info.id}";
       questIsPlay.text = $"{quest.info.DisplayName}";

       questGoal.text = quest.step.GetProgressText();
       questReward.text = $"보상 : 골드 + {quest.info.goldReward}   EXP + {quest.info.expReward}";
    }
    private void ShowFinishQuestDetail(Quest quest)
    {
        questName.text = $"-<s>{quest.info.id}</s>";
        questIsPlay.text = $"<s>{quest.info.DisplayName}</s>";
        questGoal.text = "";
        questReward.text = "";
    }
    private void SetButtonColor(Button selected)
    {
        Color green = new Color(0, 1f, 0, 1f);
        Color white = Color.white;

        // 진행중 버튼 색 변경
        ColorBlock proColor = progressBTN.colors;
        proColor.normalColor = (selected == progressBTN) ? green : white;
        proColor.selectedColor = proColor.normalColor;
        progressBTN.colors = proColor;

        // 완료한 버튼 색 변경
        ColorBlock finColor = finishBTN.colors;
        finColor.normalColor = (selected == finishBTN) ? green : white;
        finColor.selectedColor = finColor.normalColor;
        finishBTN.colors = finColor;
    }
}
