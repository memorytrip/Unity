using FMODUnity;
using UnityEngine;
using WebSocketSharp;

public class PlayOneshotSfx : MonoBehaviour
{
    [SerializeField] private EventReference eventReference;

    public void PlayOneshot()
    {
        if (eventReference.ToString().IsNullOrEmpty())
        {
            Debug.LogWarning("FMOD EventReference is null!");
            return;
        } 
        
        RuntimeManager.PlayOneShot(eventReference);
    }
}
