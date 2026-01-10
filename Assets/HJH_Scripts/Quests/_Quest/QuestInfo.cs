using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfo", menuName = "ScriptableObject/QuestInfo",order =1)]  //퀘스트 정보
public class QuestInfo : ScriptableObject
{
 [field : SerializeField] public string id { get; private set; }

    [Header("설명")]
    public string DisplayName;

    [Header("전제 조건")] 
    public int levelRequirement;
    public QuestInfo[] questPrerequistes;

    [Header("Steps")]
    public GameObject questStepPrefab;

    [Header("보상")]
    public int goldReward;
    public int expReward;




    private void OnValidate() // 스크립트 또는 인스펙터 상에서 변수의 값이 변경될 때 호출되는 함수 

                                          /*
                                           =>  Unity 에디터에서 인스펙터에서 이 스크립트 또는 해당 GameObject와 관련된 내용을 수정할 때마다 OnValidate 메소드가 호출되어 
                                                 id 변수를 해당 GameObject의 이름으로 설정하고 스크립트의 대상 객체를 dirty로 표시하여 Unity가 변경 사항을 저장하도록 유도
                                           */
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}

