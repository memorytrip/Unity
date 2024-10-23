using UnityEngine;

public class YamiQuest : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        Destroy(gameObject);
    }
}
