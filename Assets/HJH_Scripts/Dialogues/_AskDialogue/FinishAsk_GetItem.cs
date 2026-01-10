using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAsk_GetItem : MonoBehaviour
{
    public AskPoint askpoint;
    
    private void OnEnable()
    {
        askpoint.OnFinishAsk += GetItem;
    }
    private void OnDisable()
    {
        askpoint.OnFinishAsk -= GetItem;
    }
    public void GetItem()
    {
        Debug.Log("1번 대화 완료 이벤트 발생");
    }
}

