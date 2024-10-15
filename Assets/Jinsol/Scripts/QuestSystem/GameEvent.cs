namespace Jinsol
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    
    [Serializable]
    [CreateAssetMenu(fileName = "GameEvent", menuName ="ScriptableObjects/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField] private List<GameEventListener> listeners = new();

        private void Raise()
        {
            foreach (var listener in listeners)
            {
                listener.OnEventRaised();
            }
        }

        // TODO: Register/Unregister는 한 번에 하면 안될까
        public void RegisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener)) return;
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
    }
}