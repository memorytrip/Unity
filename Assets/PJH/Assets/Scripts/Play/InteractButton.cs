using Common.Network;
using UnityEngine;

public class InteractButton : MonoBehaviour, IListener
{
    private CanvasGroup cg;
    
    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
    }

    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EventType.eRaycasting:
                if (param is PlayerInteraction.RaycastInfo raycastInfo)
                { 
                    gameObject.SetActive(raycastInfo.isHit);
                }
                break;
        }
    }
}
