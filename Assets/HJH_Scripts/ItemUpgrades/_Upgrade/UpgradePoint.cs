using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(AudioSource))]
public class UpgradePoint : MonoBehaviour
{
    Animator ani;
   /* [Header("장비 강화 창")]
    public GameObject UpgradeWindow;
    public GameObject TransitionWindow;
    public GameObject MidleWindow;*/

    [Header("카메라 제어")]
    public GameObject UpgradeCameras;

    bool mainCMonoff = true;
    bool upGradeonoff = false;

    Transform player;
    float distance;
    float angleView;
    Vector3 direction;

    AudioSource audioS;

    public UpgradeStatManager upgradeStatManager;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        audioS = this.gameObject.GetComponent<AudioSource>();
    }
    private void Start()
    {
        //  SetupInitialUI();
        UpgradeCameras.SetActive(upGradeonoff);
        //  GameEventManager.instance.inputEvents.OnUpgradeWindow_On_Off += UpgradeWindow_On_Off;

        GameEventManager.instance.inputEvents.interactionEvents.onSubmitPressed += UpgradeWindow_On_Off;

    }
  /*  void SetupInitialUI()
    {
        //upGradeonoff = false
        UpgradeCameras.SetActive(upGradeonoff); // 장비 강화 전용 카메라 
        UpgradeWindow.SetActive(upGradeonoff); // 장비 강화 UI 
        MidleWindow.SetActive(false); // 장비 계승 UI
    }*/

    bool NearView() // 시야 체크
    {
        distance = Vector3.Distance(transform.position, player.transform.position); //거리 재기
        direction = transform.position - player.transform.position;// player과 몬스터의 사이값
        angleView = Vector3.Angle(player.transform.forward, direction); //바라보는 방향의 회전값 구하기
        if (angleView < 45f && distance < 5f)
            return true;
        else return false;
    }


    private void UpgradeWindow_On_Off() // 강화 Ui 껏다 키기
    {   
       if (!NearView()) return; // NPC와 거리계산 
       if (upgradeStatManager.isEffecting ||
       upgradeStatManager.isUpgrading) return; // 강화 진행 중 이라면 X
       SoundManager.instance.Play(GAMESOUND.Interaction, audioS);
       InventoryManager.instance.InventoryKey();
       ani.SetTrigger("isGreet");
       CameraManager.instance.CameraChange(this.UpgradeCameras);
       upGradeonoff = !upGradeonoff;
       UpgradeUiManager.instance.SetupInitialUI(upGradeonoff); 
    }

   
}
