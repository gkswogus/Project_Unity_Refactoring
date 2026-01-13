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
    [SerializeField] UnityEngine.UI.Button progressBTN;
    [SerializeField] UnityEngine.UI.Button finishBTN;

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
    //******************************************************************// 이하 보상 UI  
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
    //******************************************************************// 이하 정보 UI
    private void QuestWindowOn_Off() 
    {
        isQustUI = !isQustUI;
        questUI.SetActive(isQustUI);
        UIEventControll.instance.isOnUI = UIEventControll.instance.isOnUI ? false : true; // 마우스 커서 잠금
        ProgressBtn();
    }
    public void ProgressBtn() // 진행중인 퀘스트 버튼 클릭
    {
        SetButtonColor(progressBTN);
        inp_questListContent.gameObject.SetActive(true);
        fin_questListContent.gameObject .SetActive(false);
        ClearQuestDetail();
    }
    public void FinishBtn() // 완료한 퀘스트 버튼 클릭
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
         // 진행중인 퀘스트의 이름 버튼오브젝트에 함수 연결
            go.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowQuestDetail(quest));

            inProgressQuest.Add(quest.info.id, go);      
    }

    void Quest_Finish(Quest quest) 
    {      
            //진행중인 퀘스트 버튼오브젝트 및 딕셔너리에서 삭제
            if (inProgressQuest.TryGetValue(quest.info.id, out GameObject inGo))
            {
                Destroy(inGo);
                inProgressQuest.Remove(quest.info.id);
            }

           GameObject go = Instantiate(questidPrefab, fin_questListContent);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            text.text = quest.info.id;

            go.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowFinishQuestDetail(quest));
    }

    private void ClearQuestDetail()
    {
        questName.text = "";
        questIsPlay.text = "";
        questGoal.text = "";
        questReward.text = "";
    }
    private void ShowQuestDetail(Quest quest) // 진행중인 퀘스트 정보 띄우기
    {
       questName.text = $"-{quest.info.id}";
       questIsPlay.text = $"{quest.info.DisplayName}";

       questGoal.text = quest.step.GetProgressText();
       questReward.text = $"보상 : 골드 + {quest.info.goldReward}   EXP + {quest.info.expReward}";
    }
    private void ShowFinishQuestDetail(Quest quest) // 완료한 퀘스트 정보 띄우기
    {
        questName.text = $"-<s>{quest.info.id}</s>";
        questIsPlay.text = $"<s>{quest.info.DisplayName}</s>";
        questGoal.text = "";
        questReward.text = "";
    }
    private void SetButtonColor(UnityEngine.UI.Button selected)
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
