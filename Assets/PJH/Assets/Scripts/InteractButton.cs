using Common.Network;
using UnityEngine;

public class InteractButton : MonoBehaviour, IListener
{
    private void Start()
    {
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
    }

    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EventType.eRaycasting:
                if (param is PlayerInteraction.RaycastInfo raycastInfo)
                {
                    Debug.Log(Connection.StateAuthInstance.playerRef.PlayerId);
                    gameObject.SetActive(raycastInfo.isHit);
                }
                break;
        }
    }
}
