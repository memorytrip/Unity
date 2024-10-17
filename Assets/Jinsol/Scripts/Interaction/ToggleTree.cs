using UnityEngine;

public class ToggleTree : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        Debug.Log($"Player entered {gameObject}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log($"Player exited {gameObject}");
    }
}
