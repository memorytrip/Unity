using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Common.Network
{
    /**
     * Photon 접속자마다 생성되어 각 접속자 정보, 아바타 관리하는 오브젝트
     * DontDestroyOnLoad
     */
    public class Connection : NetworkBehaviour, IPlayerLeft
    {
        public static Connection StateAuthInstance;

        [Networked] public NetworkObject currenctCharacter { get; set; }
        [Networked] public string playerName { get; set; }
        [Networked] public PlayerRef playerRef { get; set; }
        [Networked] public bool hasSceneAuthority { get; set; }

        public static List<Connection> list = new ();

        public override void Spawned()
        {
            if (!HasStateAuthority) 
                return;
            Init();
            DontDestroyOnLoad(gameObject);
            SpawnAvatar();
        }

        private void OnEnable()
        {
            list.Add(this);
        }

        private void OnDisable()
        {
            list.Remove(this);
        }
        

        private void Init()
        {
            playerName = SessionManager.Instance.currentSession?.user.nickName;
            Debug.Log($"Connection Init PlayerName : {playerName}");
            
            StateAuthInstance = this;
            playerRef = Runner.LocalPlayer;
            hasSceneAuthority = Runner.IsSceneAuthority;
        }

        private void SpawnAvatar() => SpawnAvatarAsync().Forget();
        
        private async UniTaskVoid SpawnAvatarAsync()
        {
            await UniTask.WaitUntil(() => SceneManager.Instance.curScene != null);
            switch (SceneManager.Instance.curScene)
            {
                
                case "SpecialSquare":
                case "Square":
                    currenctCharacter = await SpawnProcess("Player", new Vector3(0f, 2f, -22f), Quaternion.identity);
                    break;
                case "MyRoom":
                    currenctCharacter = await SpawnProcess("Player", new Vector3(0, 5, 0), Quaternion.identity);
                    currenctCharacter.transform.Find("Scale").transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case "PlayReady":
                    currenctCharacter = await SpawnProcess("PlayReadyState");
                    SceneManager.Instance.OnSceneLoaded += SpawnAvatar;
                    break;
                case "FindPhoto":
                    SceneManager.Instance.OnSceneLoaded -= SpawnAvatar;
                    await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
                    currenctCharacter = await SpawnProcess("Player", new Vector3(0, 5, 0), Quaternion.identity);
                    currenctCharacter.transform.Find("Scale").transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                default:
                    break;
            }
        }
        
        
        public async UniTask<NetworkObject> SpawnProcess(string prefabName)
        {
            return await SpawnProcess(prefabName, Vector3.zero, Quaternion.identity);
        }
        
        public async UniTask<NetworkObject> SpawnProcess(string prefabName, Vector3 position, Quaternion rotation) 
        {
            GameObject prefab = await Resources.LoadAsync<GameObject>("Prefabs/" + prefabName) as GameObject;
            // Debug.Log($"Runner: {Runner}, Prefab: {prefab}");
            Debug.Log($"Connection count: {Connection.list.Count}");
            return await Runner.SpawnAsync(prefab, position, rotation);
        }

        public void PlayerLeft(PlayerRef player)
        {
            hasSceneAuthority = Runner.IsSceneAuthority;
        }
        
        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RpcChatMsg(string msg)
        {
            Chat.Instance.Append(msg);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RpcNewChatMsg(string msg)
        {
            NewChat.Instance.InstantiateChat(msg);
        }
    }
}