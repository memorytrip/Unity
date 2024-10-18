using System;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class InteractButton : MonoBehaviour, IListener
{
    private void Start()
    {
        EventManager.Instance.AddListener(Event_Type.eRaycasting, this);
    }

    public void OnEvent(Event_Type eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case Event_Type.eRaycasting:
                if (param == null)
                {
                    Debug.Log("없는데요?");
                }
                else
                {
                    gameObject.SetActive((bool)param);
                }
                break;
        }
    }
}
