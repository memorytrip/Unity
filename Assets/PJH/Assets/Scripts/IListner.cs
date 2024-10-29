using UnityEngine;

public enum EventType
{
    eRaycasting,
    eUI,
}
public interface IListener
{
    void OnEvent(EventType eventType, Component sender, object param = null);
}
