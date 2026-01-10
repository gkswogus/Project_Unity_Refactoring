using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using System.Data;
using static Cinemachine.DocumentationSortingAttribute;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UpgradeStatManager : MonoBehaviour, IDropHandler
{
    private STD playerGold;
    private ItemUIObject item;

    private int upgradeCost; // 강화 비용
    private int success; // 강화 확률
    private int maxItemLevel = 5; // 현 아이템 최대 레벨

    public bool isUpgrading; 
    public bool isEffecting;

    private Animator ani;

    [Header("강화 이펙트")]
    public GameObject[] UpgradeEffect;
    public Animation[] Effect;

    [SerializeField] private UpgradeData UpgradeData;
    private void Awake()
    {
        playerGold = GameObject.FindWithTag("Player").GetComponent<STD>();
        ani = GameObject.Find("NPC(대장장이 찐)").GetComponent<Animator>();

        foreach (var effect in UpgradeEffect)
            effect.SetActive(false);
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            UpgradeUiManager.instance.EmptyUpgradeText();
        }
    }
  
    private ItemUIObject GetItem() // OnDrop() 호출
    {
        if (transform.childCount == 0) return null;
        return transform.GetChild(0).GetComponent<ItemUIObject>();
    }
 
    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDrag.draggingItem.tag == "Untagged") return;

        if (this.transform.childCount==1)
        {      
            TryswapItem();
        }
        else if (transform.childCount == 0)
        {
            ItemDrag.draggingItem.transform.SetParent(this.transform);
        }    
        item = GetItem();

        UpdateItemName(); // 아이템 이름 (드롭때만 호출)

        UpdateItemLevel(); // 아이템 레벨 (강화성공 시 호출)
        UpdateElement(); // 강화 성공 확률, 강화 코스트 (강화성공 시 호출)
        UpdateStats(); // 현재 능력치, 강화 성공 시 능력치 (강화성공 시 호출 )
                       
        UpdateCurGold(); // 보유 골드 (강화 버튼 클릭 시 호출)
    }
    public void TryswapItem()
    {

        if (ItemDrag.draggingItem != null)
        {
            ItemDrag.draggingItem.transform.SetParent(this.transform);
            transform.GetChild(0).SetParent(transform.GetChild(1).GetComponent<ItemDrag>().copyItemtr);
        }

    }
    private void UpdateItemName()
    {
        UpgradeUiManager.instance.DropItemNameText(item);
    }
    private void UpdateItemLevel()
    {
        UpgradeUiManager.instance.DropItemLevelText(item.ItemLevel);
    }
    private void UpdateCurGold()
    {
        UpgradeUiManager.instance.DropItemCurGold(playerGold); 
    }


    ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   /// 

    private Dictionary<(ITEMTYPE, ITEM_RATING), int> baseUpgradeValues = new()
    {
        { (ITEMTYPE.Weapon, ITEM_RATING.Normal), 40 },
        { (ITEMTYPE.Weapon, ITEM_RATING.Epic), 50 },
        { (ITEMTYPE.Weapon, ITEM_RATING.Legend), 60 },

        { (ITEMTYPE.Shield, ITEM_RATING.Normal), 10 },
        { (ITEMTYPE.Shield, ITEM_RATING.Epic), 15 },
        { (ITEMTYPE.Shield, ITEM_RATING.Legend), 20 },

        { (ITEMTYPE.Shose, ITEM_RATING.Normal), 2 },
        { (ITEMTYPE.Shose, ITEM_RATING.Epic), 4 },
        { (ITEMTYPE.Shose, ITEM_RATING.Legend), 6 },

        { (ITEMTYPE.Hat, ITEM_RATING.Normal), 5 },
        { (ITEMTYPE.Hat, ITEM_RATING.Epic), 10 },
        { (ITEMTYPE.Hat, ITEM_RATING.Legend), 15 },
    };

    public int GetUpgradeValue(ItemUIObject item, int level) // 강화 성공 시 증가할 능력치
    {
        if (baseUpgradeValues.TryGetValue((item.ITEM_TYPE, item.ITEM_RATING), out int upValue))
        {
            return upValue + (int)(upValue * 0.1f * level);
            // 아이템 레벨(강화 단계)이 증가함에 따라 능력치도 upValue 에서 10%씩 증가함
        }
        return 0;
    }

    private void UpdateStats() //장비 종류와 등급에 따른
                               //현재 능력치와 강화 성공시 증가하는 능력치 업데이트
    {
        int nextUpgradeValue = GetUpgradeValue(item, item.ItemLevel);
        UpgradeUiManager.instance.DropItemStatText(item, nextUpgradeValue);  
    
    }
 


    private void UpgradeItem() // 강화 성공 시 호출.
    {
        // maxItemLevel => 5
        if (item.ItemLevel < maxItemLevel) 
        {
            item.value += GetUpgradeValue(item, item.ItemLevel);
            item.ItemLevel++;           
        }
        UpdateItemLevel(); // 아이템 레벨 UI 변경
        UpdateStats(); // 아이템 능력치 UI 변경
        UpdateElement(); // 강화 비용, 확률 변경
        StartCoroutine(ClearEffect()); // 성공 이펙트 재생
    }

    ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///
    public void UpgradeBtn() // 강화 버튼 클릭
    {
  // 조건을 만족하지 못하면 버튼을 눌러도 코루틴 함수 실행 안함
        if (item == null || item.ItemLevel >= maxItemLevel || 
            playerGold.money < upgradeCost) return;     
        StartCoroutine(Upgrade());
    }

    private IEnumerator Upgrade() 
    {
        if (isUpgrading) yield break; // 연속 클릭 막아주는 기능
        isUpgrading = true;
    
        ani.SetTrigger("upGrad"); // NPC 강화 애니메이션 재생
        StartCoroutine(PlayUpgradeSound()); // 강화 사운드 재생
        // 애니메이션 및 사운드 재생 시간 
        yield return new WaitForSeconds(2.2f);

        playerGold.money -= upgradeCost;
        UpdateCurGold(); // 현재 보유 골드 UI

        // 아이템 강화 확률
        bool upgradeSuccess = Random.value < (success / 100f); 
     
        if (upgradeSuccess) // 강화가 성공 했다면
        {
            UpgradeItem(); // 수치 및 UI 갱신
        }
        else
        {
            StartCoroutine(FillEffect()); // 실패 이펙트 재생
        }

        isUpgrading = false;    
    }

    /* private void SetElement(ITEM_RATING rating, int level)  
  {
      int[] goldCosts, successRates; // 아이템 레벨 별 강화 소모 골드와, 확률
      if (rating == ITEM_RATING.Legend)
      {
          goldCosts = new[] { 500, 600, 700, 800, 950, 0 };
          successRates = new[] { 50, 33, 25, 20, 16, 0 };
      }
      else if (rating == ITEM_RATING.Epic)
      {
          goldCosts = new[] { 100, 125, 150, 180, 250, 0 };
          successRates = new[] { 50, 33, 25, 20, 16, 0 };
      }
      else
      {
          goldCosts = new[] { 50, 65, 80, 100, 150, 0 };
          successRates = new[] { 50, 33, 25, 20, 16, 0 };
      }
      if (level < goldCosts.Length) // 아이템 강화 레벨 
      {
          upgradeCost = goldCosts[level];
          success = successRates[level];
          UpgradeUiManager.instance.DropItemLevelText(level);

      } 

  } */

      private void SetElemtents(ITEM_RATING rating , int level)
    {
        upgradeCost = UpgradeData.upgradeGoldCost[rating][level];
        success = UpgradeData.successRates[rating][level]; 
    }
    private void UpdateElement() // 소모골드, 확률 업데이트        
    {       
        SetElemtents(item.ITEM_RATING, item.ItemLevel); 
        UpgradeUiManager.instance.DropItemUpgradeInfoText(upgradeCost, success);
    }
  
/*
    private Dictionary<ITEM_RATING, int[]> upgradeGoldCost = new() 
    {
        {ITEM_RATING.Normal, new[] { 50, 65, 80, 100, 150, 0 } },
        {ITEM_RATING.Epic, new[] { 100, 125, 150, 180, 250, 0 } },
        {ITEM_RATING.Legend, new[] { 500, 600, 700, 800, 950, 0 } }
    };

    private Dictionary<ITEM_RATING, int[]> successRates = new()
    {    
        {ITEM_RATING.Normal, new[] { 100, 90, 80, 70, 60, 0 } },
        {ITEM_RATING.Epic, new[] { 90, 80, 70, 60, 50, 0 } },
        {ITEM_RATING.Legend, new[] { 80, 60, 40, 20, 10, 0 } }
    };
*/


    private IEnumerator PlayUpgradeSound()
    {
        SoundManager.instance.Play(UISOUND.UpgradeBtn);
        yield return new WaitForSeconds(1.0f);
        SoundManager.instance.Play(UISOUND.UpgradeBtn);
    }

    private IEnumerator ClearEffect()
    {
        isEffecting = true;
        UpgradeEffect[0].SetActive(true);
        Effect[0].Play();
        SoundManager.instance.Play(UISOUND.UpgradeO);
        yield return new WaitForSeconds(2.2f);
        UpgradeEffect[0].SetActive(false);
        isEffecting = false;
    }

    private IEnumerator FillEffect()
    {
        isEffecting = true;
        UpgradeEffect[1].SetActive(true);
        Effect[1].Play();
        SoundManager.instance.Play(UISOUND.UpgradeX);
        yield return new WaitForSeconds(2.2f);
        UpgradeEffect[1].SetActive(false);
        isEffecting = false;
    }
}