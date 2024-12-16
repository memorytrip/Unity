using UnityEngine;

namespace Common
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager Instance = null;
        
        public enum State
        {
            Lobby,
            Playing,
            Myroom,
        }

        [HideInInspector] public State state;
        public PersistenceData data;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);
// #if !UNITY_EDITOR
//             Debug.unityLogger.logEnabled = false;
// #endif

            Application.targetFrameRate = 30;
        }
    }
}