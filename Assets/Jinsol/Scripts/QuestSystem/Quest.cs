namespace Jinsol
{
    using UnityEngine;

    public class Quest
    {
        public Quest(QuestInfoSO questInfo)
        {
            _questInfo = questInfo;
            _questState = EQuestState.QuestRequirementsNotMet;
            _currentQuestStepIndex = 0;
        }
        
        private QuestInfoSO _questInfo;
        private EQuestState _questState;
        private int _currentQuestStepIndex;
        // TODO: ?
        private bool _isQuestStep;

        private void MoveToNextStep()
        {
            _currentQuestStepIndex++;
        }

        private bool _currentStepIsValid()
        {
            return (_currentQuestStepIndex < _questInfo.questStepPrefabs.Length);
        }

        private void InstantiateCurrentQuestStep(Transform parentTransform)
        {
            var questStepPrefab = GetCurrentQuestStepPrefab();

            if (questStepPrefab == null) return;
            var questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(_questInfo.questId);
        }

        private GameObject GetCurrentQuestStepPrefab()
        {
            GameObject questStepPrefab = null;
            if (_currentStepIsValid())
            {
                questStepPrefab = _questInfo.questStepPrefabs[_currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning("No quest step prefab found");
            }
            
            return questStepPrefab;
        }
    }
}