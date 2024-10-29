using System;
using UnityEngine;

public class YamiQuestManager : MonoBehaviour
{
    private QuestPopup _questPopup;
    
    private static YamiQuestManager _instance;
    public static YamiQuestManager Instance 
    { 
        get 
        { 
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<YamiQuestManager>();
            }
            return _instance; 
        } 
    }
    
    public event Action CompleteQuest;

    private void OnQuestComplete()
    {
        CompleteQuest?.Invoke();
    }
    
    public int CurrentQuestIndex { get; private set; }
    private const int RequiredQuestNumber = 7;

    private bool AllQuestsCleared()
    {
        if (CurrentQuestIndex == RequiredQuestNumber)
        {
            return true;
        }

        return false;
    }

    private void Awake()
    {
        _questPopup = FindAnyObjectByType<QuestPopup>();
        CompleteQuest += ProceedQuest;
    }

    private void CheckState()
    {
        if (AllQuestsCleared())
        {
            ShowUi(_questPopup.allClearPopup);
            Debug.Log("All Quests Cleared!");
        }
        else
        {
            ShowUi(_questPopup.questPopup);
            Debug.Log("One Quest Completed");
        }
    }

    private void ShowUi(CanvasGroup target)
    {
        target.alpha = 1f;
        target.interactable = true;
        target.blocksRaycasts = true;
    }

    private void HideUi(CanvasGroup target)
    {
        target.alpha = 0f;
        target.interactable = false;
        target.blocksRaycasts = false;
    }
    
    public void ProceedQuest()
    {
        CurrentQuestIndex++;
        CheckState();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (CurrentQuestIndex < RequiredQuestNumber)
        {
            OnQuestComplete();
        }
    }
}