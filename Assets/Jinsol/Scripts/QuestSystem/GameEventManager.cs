using UnityEngine.Serialization;

namespace Jinsol
{
    using UnityEngine;

    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance { get; private set; }

        [FormerlySerializedAs("PlayerEvents")] public PlayerEvent playerEvent;
        [FormerlySerializedAs("QuestEvents")] public QuestEvent questEvent;
        [FormerlySerializedAs("TokenEvents")] public TokenEvent tokenEvent;
        //public MiscEvents MiscEvents;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one instance of GameEventManager!");
            }

            Instance = this;
            DontDestroyOnLoad(this);

            playerEvent = new();
            questEvent = new();
            tokenEvent = new();
            //MiscEvents = new();
        }
    }
}