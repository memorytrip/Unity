using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FmodTriggerOneshotManager : MonoBehaviour
{
    private EventReference _eventRef;
    [SerializeField] private EventReference popupEvent;
    [SerializeField] private EventReference cancelEvent;
    [SerializeField] private EventReference confirmEvent;
    [SerializeField] private EventReference flipCardUpEvent;
    [SerializeField] private EventReference flipCardDownEvent;
    [SerializeField] private EventReference showMoreEvent;
    [SerializeField] private EventReference yggdrasilEvent;

    private void Start()
    {
        EventManager.Instance.OpenPopup += PopupOneshot;
        EventManager.Instance.ClosePopup += CancelOneshot;
        EventManager.Instance.TriggerYggdrasil += YggdrasilOneshot;
    }

    private void YggdrasilOneshot()
    {
        TriggerOneshot(yggdrasilEvent);
    }

    private void PopupOneshot()
    {
        TriggerOneshot(popupEvent);
    }

    private void CancelOneshot()
    {
        TriggerOneshot(cancelEvent);
    }

    public void TriggerOneshot(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
    }
}