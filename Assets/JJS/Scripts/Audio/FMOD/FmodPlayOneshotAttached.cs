using FMODUnity;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class FmodPlayOneshotAttached : MonoBehaviour
{
    private Button _button;
    [SerializeField] private EventReference eventReference;
    [SerializeField] private bool hasTriggerCollider;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayOneshot);
    }
    
    private void PlayOneshot()
    {
        RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.GetComponent<NetworkObject>().HasStateAuthority)
        {
            return;
        }
        if (hasTriggerCollider)
        {
            PlayOneshot();
        }
    }
}
