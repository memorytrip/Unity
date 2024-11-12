using System;
using UnityEngine;

[Serializable]
// TODO: 확정되면 적당하게 수정하기
[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "QuestSystem/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    public string QuestId { get; private set; }
        
    [Header("퀘스트 제목")]
    public string questName;
    [Header("UI 표시 설명")]
    public string message;
    [Header("보상 연결")] public RewardSO reward;
    [Space(10f)]
    [Header("퀘스트 내용 - 내부 식별용")]
    [TextArea(10, 15)] public string questDescription;

    [Header("퀘스트 달성 여부")] public bool questClear;
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        QuestId = name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
