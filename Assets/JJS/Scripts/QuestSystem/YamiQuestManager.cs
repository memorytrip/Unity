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

    [SerializeField] private GameObject questPopup;
    [SerializeField] private GameObject allClearPopup;
    
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
            _questPopup.allClearPopup.SetActive(true);
            Debug.Log("All Quests Cleared!");
        }
        else
        {
            _questPopup.questPopup.SetActive(true);
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