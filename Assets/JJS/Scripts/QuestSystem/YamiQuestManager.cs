using System;
using System.Collections;
using UnityEngine;

public class YamiQuestManager : MonoBehaviour
{
    public enum QuestName
    {
        FirstVisit,
        VisitTree,
        PlayGame,
        EnterMyRoom,
        VisitPhotoSpot,
        VisitBooth,
        TakeScreenshot,
        CompleteStampRally,
    }

    public enum QuestState
    {
        OneQuestCleared,
        AllQuestsCleared
    }
    
    private YamiQuestBridge _yamiQuestBridge;
    [SerializeField] private QuestInfoSO finalQuestData;
    
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

    public void OnQuestComplete()
    {
        CompleteQuest?.Invoke();
    }
    
    public int CurrentQuestIndex { get; private set; }
    private const int RequiredQuestNumber = 8;
    
    public bool AllQuestsCleared()
    {
        if (CurrentQuestIndex == RequiredQuestNumber - 1)
        {
            return true;
        }

        return false;
    }

    private void Awake()
    {
        _yamiQuestBridge = FindAnyObjectByType<YamiQuestBridge>();
        CompleteQuest += ProceedQuest;
    }

    private void ProceedQuest()
    {
        CurrentQuestIndex++;

        if (CurrentQuestIndex >= RequiredQuestNumber)
        {
            CurrentQuestIndex = RequiredQuestNumber - 1;
            Debug.LogWarning("Something's wrong here?????");
        }
    }

    public void GiveFinalReward()
    {
        StartCoroutine(ProcessFinalReward());
    }
    
    private IEnumerator ProcessFinalReward()
    {
        yield return new WaitForSeconds(2f);
        _yamiQuestBridge.SetData(true, finalQuestData.questName, finalQuestData.reward.rewardSprite, finalQuestData.message);
        _yamiQuestBridge.ShowUi(true);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (CurrentQuestIndex < RequiredQuestNumber)
        {
            OnQuestComplete();
        }
    }*/
}