using UnityEngine;

public enum Event_Type
{
    eRaycasting,
    eUI,
}
public interface IListener
{
    void OnEvent(Event_Type eventType, Component sender, object param = null);
}
