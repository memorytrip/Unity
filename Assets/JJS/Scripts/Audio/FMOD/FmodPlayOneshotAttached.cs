using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class FmodPlayOneshotAttached : MonoBehaviour
{
    private Button _button;
    [SerializeField] private EventReference eventReference;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayOneshot);
    }
    
    private void PlayOneshot()
    {
        RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
    }
}
