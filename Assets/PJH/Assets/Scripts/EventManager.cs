using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance
    {
        get { return instance; }
    }
    
    private static EventManager instance = null;
    private Dictionary<EventType, List<IListener>> Listeners = new();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void AddListener(EventType eventType, IListener listener)
    {
        if (Listeners.TryGetValue(eventType, out var ListenList))
        {
            ListenList.Add(listener);
            return;
        }

        ListenList = new List<IListener>();
        ListenList.Add(listener);
        Listeners.Add(eventType, ListenList);
    }

    public void PostNotification(EventType eventType, Component sender, object param = null)
    {
        if (!Listeners.TryGetValue(eventType, out var ListenList))
        {
            return;
        }

        for (int i = 0; i < ListenList.Count; i++)
        {
            ListenList?[i].OnEvent(eventType, sender, param);
        }
    }

    public void RemoveEvent(EventType eventType) => Listeners.Remove(eventType);

    public void RemoveRedundancies()
    {
        Dictionary<EventType, List<IListener>> newListeners = new Dictionary<EventType, List<IListener>>();

        foreach (KeyValuePair<EventType, List<IListener>> Item in Listeners)
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

    public event Action LoadCompleteMap;

    public void OnMapLoadedComplete()
    {
        Debug.Log("Map has finished loading");
        LoadCompleteMap?.Invoke();
    }

    public event Action<int> FindPhoto;

    public void OnPhotoFound(int count)
    {
        Debug.Log("Photo found");
        FindPhoto?.Invoke(count);
    }

    public event Action TriggerYggdrasil;

    public void OnYggdrasilTriggered()
    {
        TriggerYggdrasil?.Invoke();
    }

    public event Action ClosePopup;

    public void OnPopupClosed()
    {
        ClosePopup?.Invoke();
    }
    
    public event Action OpenPopup;
    public void OnPopupOpened()
    {
        OpenPopup?.Invoke();
    }

    public event Action StartWalking;

    public void OnStartedWalking()
    {
        StartWalking?.Invoke();
    }
    
    public event Action StopWalking;

    public void OnStoppedWalking()
    {
        StopWalking?.Invoke();
    }
}
