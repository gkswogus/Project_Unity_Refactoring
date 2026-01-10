using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System.IO;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using static QuestData;


public class QuestManager : MonoBehaviour
{

    private Dictionary<string, Quest> questMap;

    private int currentPlayerLevel;


    // [SerializeField] STD playerSTD;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }
  
    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventManager.instance.questEvents.onFinishQuest += FinishQuest;

        GameEventManager.instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;
      //  GameEventManager.instance.questEvents.onProgressQuestUI += ProgressQuestUI;
      //  GameEventManager.instance.questEvents.onFinishQuestUI += FinishQuestUI;
      //  GameEventManager.instance.questEvents.onRewardQuestUI += RewardQuestUI;
       // GameEventManager.instance.inputEvents.uiEvent.OnQuestWindowOn_Off += QuestWindowOn_Off;
    }

    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventManager.instance.questEvents.onFinishQuest -= FinishQuest;
        GameEventManager.instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;
      //  GameEventManager.instance.questEvents.onProgressQuestUI -= ProgressQuestUI;
      //  GameEventManager.instance.questEvents.onFinishQuestUI -= FinishQuestUI;
     //   GameEventManager.instance.questEvents.onRewardQuestUI -= RewardQuestUI;
       // GameEventManager.instance.inputEvents.uiEvent.OnQuestWindowOn_Off += QuestWindowOn_Off;
    }


    private void Start()
    {

        QuestData data = LoadQuestDataFromFile();
        LoadQuestState(data);
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.CAN_FINISH)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
                GameEventManager.instance.questEvents.Quest_inf(quest);
            }
            if(quest.state == QuestState.FINISHED) GameEventManager.instance.questEvents.Quest_fin(quest);
            GameEventManager.instance.questEvents.QuestStateChange(quest);
        }
        LoadQuestStep(data);
    }
    
    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        // 퀘스트 상태 아이콘을 바꿔주는 이벤트
        GameEventManager.instance.questEvents.QuestStateChange(quest);

       // AddQuestState(quest);
    }

    private void PlayerLevelChange(int level) 
    {
        currentPlayerLevel = level;
    }
     private bool CheckRequirementsMet(Quest quest) //퀘스트 시작 조건 확인
     {      
        bool isRequirement = true;
        // 플레이어 레벨이 퀘스트 시작 조건 레벨보다 낮은 경우
        if (currentPlayerLevel < quest.info.levelRequirement)
        {
            isRequirement = false;
        }
        // 퀘스트의 모든 선행 퀘스트가 완료 상태인지 확인
        foreach (QuestInfo PrerequisteQuestInfo in quest.info.questPrerequistes)
        {
            if (GetQuestById(PrerequisteQuestInfo.id).state != QuestState.FINISHED)
            {
                isRequirement = false;
            }
        }
        return isRequirement;
     }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
              //시작 조건을 만족한 진행 불가 상태의 퀘스트를 시작 가능 상태로 변경
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        } 
    }

   // public List<Quest> getInprogress_Q = new();
   // public List<Quest> getFinish_Q = new();
    private void StartQuest(string id) // 시작가능 => 진행중
    {
        Quest quest = GetQuestById(id);
     // 퀘스트 완료 조건인 QuestStep 오브젝트를 하위 객체로 생성
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);

     //   getInprogress_Q.Add(quest);
        GameEventManager.instance.questEvents.Quest_inf(quest);

    }

    private void AdvanceQuest(string id) // 진행중 => 완료가능
    {
        Quest quest = GetQuestById(id);
    
        ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
    }

    private void FinishQuest(string id) // 완료가능 => 완료
    {
        Quest quest = GetQuestById(id);
        QuestRewards(quest); // 퀘스트 보상 획득
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
       
     //   getInprogress_Q.Remove(quest);
     //   getFinish_Q.Add(quest);
        GameEventManager.instance.questEvents.Quest_fin(quest);

        foreach (QuestStep queststep in GetComponentsInChildren<QuestStep>())
        {
             // questId가 일치하는 경우
            if (queststep.questId == id)
            {
                // 해당 오브젝트(퀘스트) 삭제
                Destroy(queststep.gameObject);
                break; 
            }
        }
    }
    
    private void QuestRewards(Quest quest)  //보상 시스템  경험치,골드
    {
        GameEventManager.instance.goldEvents.GoldGained(quest.info.goldReward);    
        GameEventManager.instance.playerEvents.ExperienceGained(quest.info.expReward);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfo questInfo in allQuests)
        {
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest =  questMap[id];      
        return quest;
    }

 
 /*   void AddQuestState(Quest quest)
    {
        if(quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.CAN_FINISH) getInprogress_Q.Add(quest);
        if(quest.state == QuestState.FINISHED ) getFinish_Q.Add(quest);
    }*/

/*    public List<Quest> GetInprogress_Quest()  q누를때마다 리스트에 값 넣었다 뺐다 에바야.
    {
        if (questMap == null) return new List<Quest>();

        return questMap.Values
            .Where(q => q.state == QuestState.IN_PROGRESS || q.state == QuestState.CAN_FINISH)
            .ToList();
    }
   
    public List<Quest> GetFinishQuest()
    {
        if (questMap == null) return new List<Quest>();

        return questMap.Values
            .Where(q => q.state == QuestState.FINISHED)
            .ToList();
    }*/

    

    /*******퀘스트 보상창 추가.********/ // 구조분리 완료.
   /* public void RewardQuestUI(string questId)  // 혹시 이부분, 매개변수 string 말고 퀘스트 자체로 받게 => QuestPoint에 있는 퀘스트 인포 넘겨받기.
    {                                          // 이 이벤트 필요 없음. 그냥 QuestPoint에서 UIManager로 슝
        List<Quest> quests = GetInprogress_FinishQuest();
        Quest target = quests.FirstOrDefault(q => q.info.id == questId); // ← 해당 NPC의 퀘스트만
         //조건을 만족하는 첫 번째 요소를 찾을 때 유용하게 사용 FirstOrDefault
        if (target != null)
        {
           Quest_UiManager.Instance.ShowRewardText(target);
        }
    }*/

    /*******퀘스트 정보창(Q) 부분 추가.********/ 

/*    private void ProgressQuestUI(TMP_Text id, TMP_Text displayname, TMP_Text progress, TMP_Text reward)
    {
        ProgressQuestUIText progressquestui = new ProgressQuestUIText(id, displayname, progress, reward);
        Quest quest = GetProgress_FinishQuest();
        if (quest != null) progressquestui.ShowText(quest);
        else progressquestui.ShowTextEmpty();
    }

    private void FinishQuestUI(TMP_Text id)
    {
        FinishQuestUiText finishquestui = new FinishQuestUiText(id);
        List<Quest> quests = GetFinishQuest();
        // if (quests != null)
        finishquestui.ShowText(quests);
    }*/
  /*  private void QuestWindowOn_Off() // Q키로 퀘스트 정보 창 띄우기 // 이거 하나로 위에 두개 씹어먹음
    {
        List<Quest> inprogress_Qust = GetInprogress_Quest();
        List<Quest> finish_Quest = GetFinishQuest();

        Quest_UiManager.Instance.OpenQuestWindow(inprogress_Qust, finish_Quest);
    }*/

    /*******퀘스트 저장 시스템********/   //해야할 것! 하위객체에 퀘스트에있는 클리어조건 변수 가져오기! 인터페이스추가.
    //JSON 저장 (파일 기반): 복잡한 구조, 여러 개의 데이터, 리스트, 오브젝트 저장 시 적합
    public QuestData GetQuestData()  // 저장 시 담을 데이터 그릇.
    {
        QuestData data = new QuestData();
        data.quests = new List<QuestData.SavedQuestData>();

        foreach (var pair in questMap)
        {
            Quest quest = pair.Value;

            data.quests.Add(new QuestData.SavedQuestData
            {
                questId = pair.Key,
                questState = quest.state,
                stepState = quest.step?.SaveStepData(),
            });

        }
        return data;
    }
 
    public void Save() // 저장 버튼 클릭 시 호출되고, 위에서 담아온 정보를 제이슨파일에 저장
    {
        QuestData saveData = GetQuestData(); // 퀘스트 저장 데이터 가져오기
        string json = JsonUtility.ToJson(saveData, true); // JSON 문자열로 변환

        // 파일 경로 설정 (게임 디렉토리 안에 저장)
        string filePath = Application.persistentDataPath + "/quest_save.json";

        // 파일에 저장
        File.WriteAllText(filePath, json);
        //Debug.Log("Quest data saved to " + filePath);
        string filePaths = Application.persistentDataPath + "/quest_save.json";
        Debug.Log("저장 경로: " + filePaths);
    }
   
    private QuestData LoadQuestDataFromFile() // 로드 시 "제이슨 파일에 저장 돼어 있는 정보" 담을 그릇
    {
        string filePath = Application.persistentDataPath + "/quest_save.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            QuestData loadedData = JsonUtility.FromJson<QuestData>(json);
            Debug.Log("Quest data loaded from " + filePath);
            return loadedData;
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }
    public void LoadQuestState(QuestData data) // 퀘스트 상태 가져오기!
    {

        // 파일이 존재하는지 확인
        if (data == null) return;

        foreach (QuestData.SavedQuestData saved in data.quests)
        {
            if (questMap.ContainsKey(saved.questId))
            {
                Quest quest = questMap[saved.questId];
                quest.state = saved.questState;
            }
        }
    }  
    public void LoadQuestStep(QuestData data) // 퀘스트 스텝 가져오기! 
    {
        if (data == null) return;

        foreach (QuestData.SavedQuestData saved in data.quests)
        {
            if (questMap.ContainsKey(saved.questId))
            {
                Quest quest = questMap[saved.questId];
                if (quest.step != null)
                    quest.step.LoadStepData(saved.stepState);
            }
        }
    } 


    public void DeleteQuestSaveFile() // 제이슨 파일에 담긴 저장 정보 삭제
    {
        string filePath = Application.persistentDataPath + "/quest_save.json";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("퀘스트 저장 파일이 삭제되었습니다.");
        }
        else
        {
            Debug.Log("삭제할 저장 파일이 없습니다.");
        }
    } 
 
    public void ReMoveDate() // 삭제 버튼 호출
    {
         DeleteQuestSaveFile();
    }
    public void SaveDate() // 저장 버튼 호출
    {
        Save();
    }
   
}
