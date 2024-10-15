using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerNearPlayer : MonoBehaviour
{
    public enum TriggerType
    {
        Sound,
        Video,
        Npc,
        Event,
    }
        
    [SerializeField] private TriggerType triggerType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player {other.gameObject} near {gameObject}! Trigger type: {triggerType}");
        }
        else
        {
            Debug.Log($"?????");
        }
    }
}