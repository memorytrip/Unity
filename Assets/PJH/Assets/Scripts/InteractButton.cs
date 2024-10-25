using System;
using Fusion;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
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
                    gameObject.SetActive(raycastInfo.isHit);
                }
                break;
        }
    }
}
