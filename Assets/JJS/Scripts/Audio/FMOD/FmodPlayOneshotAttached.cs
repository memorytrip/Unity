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
        if (TryGetComponent(out _button))
        {
            _button.onClick.AddListener(PlayOneshot);
        }
    }
    
    public void PlayOneshot()
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