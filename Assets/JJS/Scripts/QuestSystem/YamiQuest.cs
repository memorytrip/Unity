using Fusion;
using UnityEngine;

public class YamiQuest : MonoBehaviour
{
    [SerializeField] private QuestInfoSO questData;
    private YamiQuestBridge _yamiQuestBridge;

    private void Awake()
    {
        _yamiQuestBridge = FindAnyObjectByType<YamiQuestBridge>();
    }
 
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (!other.GetComponent<NetworkObject>().HasStateAuthority)
            return;
        
        _yamiQuestBridge.SetData(YamiQuestManager.Instance.AllQuestsCleared(), questData.questName,
            questData.reward.rewardSprite, questData.message);
        _yamiQuestBridge.ShowUi(YamiQuestManager.Instance.AllQuestsCleared());
        YamiQuestManager.Instance.OnQuestComplete();
        //YamiQuestManager.Instance.ProceedQuest();
        
        if (YamiQuestManager.Instance.AllQuestsCleared())
        {
            YamiQuestManager.Instance.GiveFinalReward();
        }

        Destroy(gameObject);
    }
}