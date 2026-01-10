using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAsk_OpenDoor : MonoBehaviour
{
    public AskPoint askpoint;

    private void OnEnable()
    {
        askpoint.OnFinishAsk += OpenDoor;
    }
    private void OnDisable()
    {
        askpoint.OnFinishAsk -= OpenDoor;
    }
    public void OpenDoor()
    {
        Debug.Log("3번 대화 완료 이벤트 발생");
    }
}
