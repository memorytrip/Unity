using System;
using Fusion;
using UnityEngine;

namespace Common.Network
{
    /**
     * Fusion NetworkRunner를 관리하는 싱글톤.
     * DontDestroyOnLoad
     */
    public class RunnerManager: MonoBehaviour
    {
        public static RunnerManager Instance = null;
        public NetworkRunner Runner;
        public NetworkEvents networkEvents;
        public NetworkObjectManager networkObjectManager;

        private GameObject RunnerObject;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);
        }

        /**
         * TODO: 방 접속 후 실행할 머시깽이 넣기
         */
        public async void Connect(string roomName)
        {
            if (RunnerObject != null)
                throw new Exception("Try fusion Connect while Runner is already exist");
            
            RunnerObject = new GameObject("NetworkRunner");
            Runner = RunnerObject.AddComponent<NetworkRunner>();
            networkEvents = RunnerObject.AddComponent<NetworkEvents>();
            networkObjectManager = RunnerObject.AddComponent<NetworkObjectManager>();
            
            StartGameArgs args = new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = roomName,
                PlayerCount = 4,
                SceneManager = RunnerObject.AddComponent<NetworkSceneManagerDefault>()
            };

            StartGameResult result = await Runner.StartGame(args);

            if (result.Ok)
            {
                
            }
            else
            {
                
            }
        }

        public async void Disconnect()
        {
            if (RunnerObject == null)
                throw new Exception("Trying fusion disconnect while Runner is not exist");

            await Runner.Shutdown();
            Destroy(RunnerObject);
        }
    }
}