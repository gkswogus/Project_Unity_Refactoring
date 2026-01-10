using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;


public class AskPoint : MonoBehaviour
{
   [SerializeField] public AskData askdata;

    public static Dictionary<string, AskState> askState = new Dictionary<string, AskState>();

    private Dialoguess[] dialouge;
    private GameObject dialogueTextUi;
    private Text txt_Dialogue;
    [SerializeField] private AskState state;

    protected int curntIndex; // 대화 텍스트 인덱스
    bool isOnDialogue; // 대화창이 켜졌나?(대화진행중)

    Transform player;
    float distance;
    float angleView;
    Vector3 direction;

    private STD playerstd;

    private GameObject alarmPrefab;
    private GameObject alarmInstance;
    private void Awake()
    {
        askState[askdata.dialougeID] = AskState.UnCompleted;
        dialouge = askdata.dialogues;
          dialogueTextUi = GameObject.Find("AskCanvas");
        txt_Dialogue = GameObject.Find("대화 텍스트").GetComponent<Text>();

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerstd= GameObject.FindWithTag("Player").GetComponent<STD>();

        if (alarmPrefab == null)
        {
            alarmPrefab = Resources.Load<GameObject>("DialogueAlam/alam");
        }


        GameEventManager.instance.playerEvents.onPlayerLevelChange += PlayerLevelChanged;
    }
    void PlayerLevelChanged(int level)
    {
        playerstd.level= level;
        UpdateAlam();
    }
    void Start()
    {
        
        GameEventManager.instance.inputEvents.interactionEvents.onSubmitPressed += OpenDialogue;
        GameEventManager.instance.inputEvents.interactionEvents.onNextDialogue += NextDialogue;

        dialogueTextUi.gameObject.SetActive(false);


        UpdateAlam();
    }

    bool NearView() // 시야 체크
    {
        distance = Vector3.Distance(transform.position, player.transform.position); // 거리 재기
        direction = transform.position - player.transform.position;// player가 npc로 향하는 방향
        angleView = Vector3.Angle(player.transform.forward, direction); // 바라보는 방향의 회전값 구하기
        if (angleView < 45f && distance < 5f)
            return true;
        else return false;
    }

   bool AskOn()
    {
        if (askdata.requiredLevel > playerstd.level) return false;
        if (!string.IsNullOrEmpty(askdata.prerequisiteDialogueID))
        {
            if (askState.TryGetValue(askdata.prerequisiteDialogueID, out AskState state))
            {
                if (state == AskState.UnCompleted)
                    return false;
            }
            else return false;
        }
        
        return true;
    }
    void OpenDialogue()
    {
        // NearView => 플레이어와 NPC 사이에 거리 체크
        if (!NearView() || askState[askdata.dialougeID] == AskState.Completed) return;

        if (AskOn())
        {
            // 대화창 열기
            dialogueTextUi.gameObject.SetActive(true);
            isOnDialogue = true;
            if (alarmInstance)
            {
                Destroy(alarmInstance);
                alarmInstance = null;
            }
            curntIndex = 0; // 현재 보여줄 대사의 인덱스
            NextDialogueText(); // 첫 번째 대사부터 시작
        }     
    }
    private bool isTextFinished;
    private void NextDialogueText()
    {
        isTextFinished = false;
        // TextSlow 코루틴을 시작하여 텍스트를 한 글자씩 출력
        StartCoroutine(TextSlow());
    }
    void NextDialogue()
    {
        // 대화가 진행 중 일 때만 동작 되도록
        if (!isOnDialogue || !isTextFinished) return;

        if (dialouge.Length > curntIndex)
        {
            NextDialogueText(); // 다음 대사 출력
        }
        else if (dialouge.Length == curntIndex) // 대화 완료
        {
            dialogueTextUi.gameObject.SetActive(false);
            isOnDialogue = false;
            askState[askdata.dialougeID]= AskState.Completed;

            OnDialogueCompleted();
            FinishAsk();
           // OnDialogueComplete(); // 추상 메서드 
        }
    }
    public event Action OnFinishAsk;
    public void FinishAsk()
    {
        OnFinishAsk?.Invoke();
    }
    public void OnDialogueCompleted()
    {
        askState[askdata.dialougeID] = AskState.Completed;

        /*  // 같은 NPC 내 모든 AskPoint에게 알람 상태 갱신 요청
          AskPoint[] allPoints = GetComponents<AskPoint>();
          foreach (var point in allPoints)
          {
              point.UpdateAlam();
          }*/
        foreach (var point in AskAlram.Instance.allPoints)
        {
            point.UpdateAlam(); // 조건에 따라 알람 띄움
        }

    }
    // Update is called once per frame
    void Update()
    {
     state = askState[askdata.dialougeID];
    }

    IEnumerator TextSlow()
    {
        int textcount = 0;
        // 현재 대사 텍스트를 가져옴
        string currentDialogue = dialouge[curntIndex].dialogue1;

        txt_Dialogue.text = "";

        // 현재 대사의 길이만큼 반복하여 한 글자씩 text_Dialogue에 추가
        while (textcount < currentDialogue.Length)
        {
            txt_Dialogue.text += currentDialogue[textcount];
            textcount++;
            yield return new WaitForSeconds(0.01f);
        }
        curntIndex++; // 다음 대사로 이동
        isTextFinished = true;
    }
    void UpdateAlam()
    {
        if (alarmPrefab == null) return;
        // 이미 대화를 완료하고, 값을 저장한 뒤 게임을 로드하면 
        // 다시 느낌표가 생성되지 않게 하기 위해 return
        if (askState[askdata.dialougeID]== AskState.Completed) return;

        // 현재 NPC의 npcIndex값이 currentIndex  이하이면, 해당 NPC와 대화할 수 있는 상태로 판단합니다.
        if (AskOn())
        {
            if (alarmInstance == null)
            {
                // 알람 생성 (느낌표 프리팹)
                Transform canvasTransform = transform.Find("Canvas");
                if (canvasTransform == null) return;
                alarmInstance = Instantiate(alarmPrefab, canvasTransform);
                // 부모 좌표를 기준으로 Y값에 따른 위치 조정
                alarmInstance.transform.localPosition = new Vector3(0, 2f, 0);

            }
        }
    }
}
