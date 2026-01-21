/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkDialogueVariables
{
    private Dictionary<string, Ink.Runtime.Object> variables;

    public InkDialogueVariables(Story story)
    {
        variables = new Dictionary<string, Ink.Runtime.Object>();
        // Ink 스토리에서 변수 읽어서 딕셔너리에 저장
        foreach (string name in story.variablesState)
        {                                              // Ink 내부의 전역 변수 값 가져오기
            Ink.Runtime.Object value = story.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
        }
    }
    


    public void StartListening(Story story)
    {
        SyncVariablesToStory(story);
        // Ink 내부 이벤트 기능
        // Ink 변수 값이 바뀔 때마다 함수 호출 (퀘스트 상태)
        story.variablesState.variableChangedEvent += UpdataVariableState;
    }
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= UpdataVariableState;
    }

    public void UpdataVariableState(string name, Ink.Runtime.Object value)
    {
        if( !variables.ContainsKey(name) )
        {
            return;
        }
        variables[name] = value;
    }

    // 딕셔너리 변수들을 Ink 스토리에 동기화
    private void SyncVariablesToStory( Story story)
    {
            // 딕셔너리 반환 타입 Key-Value 구조체
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }


}
*/