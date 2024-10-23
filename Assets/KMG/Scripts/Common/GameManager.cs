using System;
using Common.Network;
using Unity.Cinemachine;
using UnityEngine;

namespace Common
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager Instance = null;
        public CinemachineCamera cinemachineCamera;
        
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
        }

        private void Start()
        {
            SceneManager.Instance.OnLoadScene += FindCamera;
        }

        private void FindCamera()
        {
            if (Connection.StateAuthInstance == null) return;
            cinemachineCamera = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();
        }
    }
}