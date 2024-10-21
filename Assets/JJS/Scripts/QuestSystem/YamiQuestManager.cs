using System;
using UnityEngine;

public class YamiQuestManager : MonoBehaviour
{
    public static YamiQuestManager Instance;
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
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
        CompleteQuest += ProceedQuest;
    }

    private void CheckState()
    {
        if (AllQuestsCleared())
        {
            Debug.Log("All Quests Cleared!");
        }
        else
        {
            Debug.Log("One Quest Completed");
        }
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