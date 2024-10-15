namespace Jinsol
{
    using System;
    using UnityEngine;

    [Serializable]
    [CreateAssetMenu(fileName = "QuestInfo", menuName = "ScriptableObjects/QuestInfo")]
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public string questId { get; private set; }
        
        [Header("퀘스트 제목")]
        [SerializeField] private string questName;
        [Header("달성 단계")]
        public GameObject[] questStepPrefabs; // TODO: 이게 최선이냐
        [Header("퀘스트 발생 조건")]
        [SerializeField] private int playerLevelRequired; // TODO: 플레이어 레벨 시스템이 있다면
        [SerializeField] private QuestInfoSO[] prerequisiteQuests;

        [Header("보상")] // TODO: 있다면 - 수집한 사진의 매수라고 봐도 무난할 듯
        [SerializeField] private int receivedTokens;

        private void OnValidate()
        {
#if UNITY_EDITOR
            questId = name;
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}