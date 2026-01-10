using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAsk_GetEx : MonoBehaviour
{
    public AskPoint askpoint;

    private void OnEnable()
    {
        askpoint.OnFinishAsk += GetEx;
    }
    private void OnDisable()
    {
        askpoint.OnFinishAsk -= GetEx;
    }
    public void GetEx()
    {
        Debug.Log("2번 대화 완료 이벤트 발생");
    }
}
