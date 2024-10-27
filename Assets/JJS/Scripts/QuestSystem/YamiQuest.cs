using UnityEngine;

public class YamiQuest : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        YamiQuestManager.Instance.ProceedQuest();
        Destroy(gameObject);
    }
}
