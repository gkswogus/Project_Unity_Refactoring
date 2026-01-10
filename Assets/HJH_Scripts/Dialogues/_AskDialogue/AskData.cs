using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

[System.Serializable]
public class Dialoguess
{
    [TextArea] // 한줄 말고 여러 줄 쓸 수 있게 해줌
    public string dialogue1; // 대사 내용을 저장하는 문자열 변수

}
[CreateAssetMenu(fileName = "AskData", menuName = "Ask/Data")]
public class AskData : ScriptableObject
{
    public string dialougeID;   // 대사 식별값
    public Dialoguess[] dialogues;              // 대사 내용
    public int requiredLevel =1;               // 플레이어 레벨 조건
    public string prerequisiteDialogueID;           // 선행 대화 
}

public enum AskState
{
         NULL,
  UnCompleted,
    Completed
}