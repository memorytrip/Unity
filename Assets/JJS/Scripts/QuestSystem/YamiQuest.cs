using Fusion;
using UnityEngine;

public class YamiQuest : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (!other.GetComponent<NetworkObject>().HasStateAuthority)
            return;
        YamiQuestManager.Instance.ProceedQuest();
        Destroy(gameObject);
    }
}
