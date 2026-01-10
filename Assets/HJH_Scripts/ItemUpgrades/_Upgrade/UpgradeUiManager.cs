using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Cinemachine.DocumentationSortingAttribute;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;

public class UpgradeUiManager : MonoBehaviour
{
    public static UpgradeUiManager instance;
    [Header("장비 강화 창")]
    [SerializeField] private  GameObject upgradeItemUi; // 강화 메인 UI
    [Header("장비 계승 창")]
    [SerializeField] private GameObject transferItemUi; // 계승 메인 UI
                      public GameObject transferingUi; // 계승 진행 중... UI
    [Header("장비 강화 창 Text")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemLevel;
    [SerializeField] private TMP_Text currStat;
    [SerializeField] private TMP_Text nextStat;
    [SerializeField] private TMP_Text currGold;
    [SerializeField] private TMP_Text upgradeGold;
    [SerializeField] private TMP_Text chance;
    [Header("계승 창 Text")]
    [SerializeField] private TMP_Text leftText;
    [SerializeField] private TMP_Text rightText;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        EmptyUI();
    }
    //******************************************// 이하 강화창 UI 부분
    private void EmptyUI() // 게임 시작 시 모든 UI 끄기
    {
        upgradeItemUi.SetActive(false);
        transferingUi.SetActive(false);
        transferItemUi.SetActive(false);
    }
    public void SetupInitialUI(bool isOnOff) // 강화 UI 켜고 끄기
    {
        upgradeItemUi.SetActive(isOnOff);
        transferItemUi.SetActive(false);
        transferingUi.SetActive(false);
    }
    public void ChangeUpgrade() // 강화 창으로 변경 버튼 클릭
    {
        transferItemUi.SetActive(false);
        upgradeItemUi.SetActive(true);
    }
    public void ChangeTransition() // 계승 창으로 변경 버튼 클릭
    {
        upgradeItemUi.SetActive(false);
        transferItemUi.SetActive(true);
    }
    //******************************************// 이하 강화창 Text 부분 
    public void EmptyUpgradeText()
    {
        currStat.text = ""; // 현재 능력치
        nextStat.text = ""; // 강화 성공시 능력치
        upgradeGold.text = ""; // 강화시 소모 골드
        chance.text = ""; // 강화 확률
        itemLevel.text = ""; // 아이템 레벨
        currGold.text = ""; // 현재 보유 골드
        itemName.text = ""; // 아이템 레벨
    }
    public void DropItemNameText(ItemUIObject item) // 아이템 이름 (1번만)
    {
        itemName.text = item.itemname;
    }
    public void DropItemCurGold(STD player) // 현재 골드 (버튼 클릭 시)
    {
        currGold.text = "현재 골드 : " + player.money;
    }
    public void DropItemLevelText(int level) // 아이템 레벨 (성공 시)
    {
        itemLevel.text = " + " + level;
    }
   
    public void DropItemStatText (ItemUIObject item, int nextValue)
    {
        currStat.text = $"{item.itemabilityInfo}\n\n {item.value}";
        nextStat.text = $"다음 능력치 \n\n + {nextValue}";
    }
    public void DropItemUpgradeInfoText(int cost, int success) // 강화 확률, 소모 골드 (강화 성공 시)
    {
       
        upgradeGold.text = "소모 골드 : " + cost;
        chance.text = "강화 확률 : " + success + "%";
    }
    //******************************************// 이하 계승창 Text 부분 
    
    public void EmptyTransferLeftText()
    {
        leftText.text = "";
    }
    public void EmptyTransferRightText()
    {
         rightText.text = "";
    }

    public void LeftItemText(ItemUIObject item)
    {
        leftText.text = $"{item.itemname}+{item.ItemLevel}";
    }
    public void RightItemText(ItemUIObject leftItem,ItemUIObject rightItem)
    {
        rightText.text = $"{rightItem.itemname}+{rightItem.ItemLevel}({leftItem.ItemLevel - 1})";
    }
    public void CantTransferText()
    {
        rightText.text = "계승 불가"; 
    }
    public void TransferCompleteText(ItemUIObject item)
    {
        rightText.text = $"{item.itemname}+{item.ItemLevel}";
    }


}
