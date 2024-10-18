namespace Draft
{
    using UnityEngine;

    public class Quest : MonoBehaviour
    {
        public Quest(QuestInfoSO questInfo)
        {
            _questInfo = questInfo;
            QuestCleared = false;
        }
        
        private QuestInfoSO _questInfo;
        public bool QuestCleared { get; private set; }

        public void ClearQuest()
        {
            QuestCleared = true;
        }
    }
}