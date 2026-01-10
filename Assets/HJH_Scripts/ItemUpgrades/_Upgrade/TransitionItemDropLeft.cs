using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TransitionItemDropLeft : MonoBehaviour, IDropHandler
{

    [Header("오른쪽 계승창")]
    [SerializeField] GameObject rightSlot;
    public void OnDrop(PointerEventData eventData)
    {
       if( rightSlot.transform.childCount==1) return; // 계승 받을 장비 슬롯에 아이템이 들어있다면 X
        //인벤창에 들어가게하는거
        if (transform.childCount == 1) 
        {
            TryswapItem();
        }
        else if (transform.childCount == 0)
        {
            ItemDrag.draggingItem.transform.SetParent(this.transform);
        }
        var item = this.transform.GetChild(0).GetComponent<ItemUIObject>();

        UpgradeUiManager.instance.LeftItemText(item);
    }
    public void TryswapItem()
    {
        if (ItemDrag.draggingItem != null)
        {
            ItemDrag.draggingItem.transform.SetParent(this.transform);
            transform.GetChild(0).SetParent(transform.GetChild(1).GetComponent<ItemDrag>().copyItemtr);
        }
    }
    private void Update()
    {
        if (this.transform.childCount == 0)
        {
            UpgradeUiManager.instance.EmptyTransferLeftText();
        }       
    }
}
