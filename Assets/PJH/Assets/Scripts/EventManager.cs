using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance
    {
        get { return instance; }
    }
    
    private static EventManager instance = null;
    private Dictionary<Event_Type, List<IListener>> Listeners = new Dictionary<Event_Type, List<IListener>>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        DestroyImmediate(gameObject);
    }

    public void AddListener(Event_Type eventType, IListener listener)
    {
        List<IListener> ListenList = null;

        if (Listeners.TryGetValue(eventType, out ListenList))
        {
            ListenList.Add(listener);
            return;
        }

        ListenList = new List<IListener>();
        ListenList.Add(listener);
        Listeners.Add(eventType, ListenList);
    }

    public void PostNotification(Event_Type eventType, Component sender, object param = null)
    {
        List<IListener> ListenList = null;

        if (!Listeners.TryGetValue(eventType, out ListenList))
        {
            return;
        }

        for (int i = 0; i < ListenList.Count; i++)
        {
            ListenList?[i].OnEvent(eventType, sender, param);
        }
    }

    public void RemoveEvent(Event_Type eventType) => Listeners.Remove(eventType);

    public void RemoveRedundancies()
    {
        Dictionary<Event_Type, List<IListener>> newListeners = new Dictionary<Event_Type, List<IListener>>();

        foreach (KeyValuePair<Event_Type, List<IListener>> Item in Listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                {
                    Item.Value.RemoveAt(i);
                }

                if (Item.Value.Count > 0)
                {
                    newListeners.Add(Item.Key, Item.Value);
                }

                Listeners = newListeners;
            }
        }
    }

    void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }
}
