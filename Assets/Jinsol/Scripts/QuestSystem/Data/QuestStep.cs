namespace Jinsol
{
    using UnityEngine;

    public class QuestStep : MonoBehaviour
    {
        private bool _completed = false;
        private string _questId;

        public void InitializeQuestStep(string questId)
        {
            _questId = questId;
        }

        protected void CompleteQuestStep()
        {
            if (_completed) return;
            _completed = true;
            //QuestEvents.OnQuestContinued(_questId);
        }
    }
}