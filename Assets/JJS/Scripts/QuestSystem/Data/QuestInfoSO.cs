using UnityEngine.Serialization;

namespace Draft
{
    using System;
    using UnityEngine;

    [Serializable]
    // TODO: 확정되면 적당하게 수정하기
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "QuestSystem/QuestInfoSO")]
    public class QuestInfoSO : ScriptableObject
    {
        public string QuestId { get; private set; }
        
        [Header("퀘스트 제목")]
        [Tooltip("예시: ????")] // TODO: 적당한 예시는 나중에 알아보도록 하자
        public string questName;

        [Header("퀘스트 달성 조건")] public QuestInfoSO[] prerequisiteQuestInfo;
        [Header("보상 연결")] public RewardSO reward;
        [Space(10f)]
        [Header("퀘스트 내용 - 내부 식별용")]
        [TextArea(10, 15)] public string questDescription;

        [Header("퀘스트 달성 여부")] public bool questClear;
        
        // TODO: 이거 왜 있는지 잊어버렸다
        private void OnValidate()
        {
#if UNITY_EDITOR
            QuestId = name;
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}