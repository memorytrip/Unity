using System;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Network
{
    /**
     * Fusion NetworkRunner를 관리하는 싱글톤.
     * DontDestroyOnLoad
     */
    public class RunnerManager: MonoBehaviour
    {
        public static RunnerManager Instance = null;

        public bool isRunnerExist
        {
            get
            {
                return Runner != null;
            }
        }

        [HideInInspector] public NetworkRunner Runner;
        [HideInInspector] public NetworkEvents networkEvents;
        [HideInInspector] public ConnectInvoker connectInvoker;

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
            
            RunnerObject = new GameObject("FusionRunner");
            Runner = RunnerObject.AddComponent<NetworkRunner>();
            networkEvents = RunnerObject.AddComponent<NetworkEvents>();
            connectInvoker = RunnerObject.AddComponent<ConnectInvoker>();
            
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
            Runner = null;
        }
    }
}