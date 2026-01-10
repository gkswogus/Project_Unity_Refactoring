using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Collections;
using UnityEngine.InputSystem.iOS;
using System.Runtime.CompilerServices;
using System.Collections.Generic;


public class TransitionItemDropRight : MonoBehaviour, IDropHandler
{

    [Header("왼쪽 계승창")]
    [SerializeField] GameObject leftSlot;

    [SerializeField] GameObject[] transitionEffects;

     private UpgradeStatManager ugradeStatManager;

    private ItemUIObject leftItem;
    private ItemUIObject rightItem;

    void Awake()
    {
        ugradeStatManager = GameObject.Find("장비 올려놓을 슬롯").GetComponent<UpgradeStatManager>();
    }
    void Start()
    {
        foreach (var effect in transitionEffects)
        {
            effect.SetActive(false);
        }
    }
    private void Update()
    {
        if (this.transform.childCount == 0)
        {
            UpgradeUiManager.instance.EmptyTransferRightText();
        }
    }

    Dictionary<ITEM_RATING, ITEM_RATING> checkRating = new()
    {
            {ITEM_RATING.Normal,ITEM_RATING.Epic },
            {ITEM_RATING.Epic, ITEM_RATING.Legend },       
    };
     
    //계승 가능 조건
    private bool CanTransfer(ItemUIObject leftItem, ItemUIObject rightItem) 
    {                       // 계승 할 아이템      // 계승 받을 아이템

        if (!checkRating.ContainsKey(leftItem.ITEM_RATING)) return false;
        if( leftItem.ITEM_TYPE != rightItem.ITEM_TYPE ) return false;
        if (leftItem.ItemLevel < rightItem.ItemLevel + 2) return false;

        return checkRating[leftItem.ITEM_RATING] == rightItem.ITEM_RATING;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (leftSlot.transform.childCount == 0) return; // 계승 할 장비 슬롯에 아이템이 없다면 X
        
            if (this.transform.childCount == 1)
            {
                TryswapItem();
            }
            else if (this.transform.childCount == 0)
            {
                ItemDrag.draggingItem.transform.SetParent(this.transform);
            }
        

        leftItem = leftSlot.transform.GetChild(0).gameObject.GetComponent<ItemUIObject>();
        rightItem = this.transform.GetChild(0).gameObject.GetComponent<ItemUIObject>();

        if (CanTransfer(leftItem, rightItem))
             UpgradeUiManager.instance.RightItemText(leftItem, rightItem);
        else UpgradeUiManager.instance.CantTransferText();
    }

    public void TryswapItem()
    {
        if (ItemDrag.draggingItem != null)
        {
            ItemDrag.draggingItem.transform.SetParent(this.transform);
            transform.GetChild(0).SetParent(transform.GetChild(1).GetComponent<ItemDrag>().copyItemtr);
        }
    }

    public void CheakItem() //계승 버튼 클릭시 실행 
    {
        if (leftItem == null || rightItem == null) return;

        if (CanTransfer(leftItem, rightItem))
        {
            StartCoroutine(ShowTransferWindow());
            Destroy(leftSlot.transform.GetChild(0).gameObject);
            rightItem.ItemLevel = leftItem.ItemLevel - 1;

            int inheritedValue = 0; // 기본 능력치 + 보너스 능력치
            for (int i = 0; i < rightItem.ItemLevel; i++) // 총 능력치 계산
            {
                inheritedValue += ugradeStatManager.GetUpgradeValue(rightItem,i);
            }
            rightItem.value = rightItem.baseValue + inheritedValue;
            // 아이템 기본 능력치
            UpgradeUiManager.instance.TransferCompleteText(rightItem);
        }
    }
   

    IEnumerator ShowTransferWindow() // 계승 진행중... 창 열기
    {
       UpgradeUiManager.instance.transferingUi.SetActive(true);
        SoundManager.instance.Play(UISOUND.Transition);

        for (int i = 0; i < transitionEffects.Length; i++) // 진행중...text
        {
            transitionEffects[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);           
        }

        yield return new WaitForSeconds(1.0f);

        UpgradeUiManager.instance.transferingUi.SetActive(false);
        foreach (var effect in transitionEffects)
        {
            effect.SetActive(false);
        }
    }

  

}