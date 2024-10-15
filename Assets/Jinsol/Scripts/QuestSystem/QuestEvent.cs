using UnityEngine;
using System;

public class QuestEvent : MonoBehaviour
{
    private event Action<string> QuestStart;

    public void OnQuestStarted(string questId)
    {
        QuestStart?.Invoke(questId);
    }

    private event Action<string> ContinueQuest;

    public void OnQuestContinued(string questId)
    {
        ContinueQuest?.Invoke(questId);
    }
        
    private event Action<string> CompleteQuest;

    public void OnQuestCompleted(string questId)
    {
        CompleteQuest?.Invoke(questId);
    }

    // TODO: 필요하지 않을 수도?
    private event Action<string> CancelQuest;

    public void OnQuestCancelled(string questId)
    {
        CancelQuest?.Invoke(questId);
    }

    // TODO: 필요하지 않을 수도?
    private event Action<string> FailQuest;

    public void OnQuestFailed(string questId)
    {
        FailQuest?.Invoke(questId);
    }

    private event Action<Quest> ChangeQuestState;

    public void OnQuestStateChanged(Quest quest)
    {
        ChangeQuestState?.Invoke(quest);
    }

    // TODO: ????????????????????????
    /*private event Action<string, int, QuestStepState> QuestStepChangeState;

    public void OnQuestStepStateChanged(string questId, int stepIndex, QuestStepState questStepState)
    {
        QuestStepChangeState?.Invoke(questId, stepIndex, questStepState);
    }*/
}