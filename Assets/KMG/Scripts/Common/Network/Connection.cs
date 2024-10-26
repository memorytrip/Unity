using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Common.Network
{
    /**
     * Photon 접속자마다 생성되어 각 접속자 정보, 아바타 관리하는 오브젝트
     * DontDestroyOnLoad
     */
    public class Connection: NetworkBehaviour, IPlayerLeft
    {
        public static Connection StateAuthInstance;

        [Networked] public NetworkObject currenctCharacter { get; set; }
        [Networked] public string playerName { get; set; }
        [Networked] public PlayerRef playerRef { get; set; }
        [Networked] public bool hasSceneAuthority { get; set; }

        public static List<Connection> list = new ();

        public Action OnAfterSpawned;

        public override void Spawned()
        {
            if (!HasStateAuthority) 
                return;
            Init();
            OnAfterSpawned?.Invoke();
            DontDestroyOnLoad(gameObject);
            SpawnAvatar().Forget();
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
            ///// BE로 교체 에정
            playerName =  PlayerPrefs.GetString("NickName", string.Empty);
            Debug.Log($"Connection Init() PlayerName : {playerName}");
            
            StateAuthInstance = this;
            playerRef = Runner.LocalPlayer;
            hasSceneAuthority = Runner.IsSceneAuthority;
        }

        private async UniTaskVoid SpawnAvatar()
        {
            await UniTask.WaitWhile(() => SceneManager.Instance.curScene == null);
            switch (SceneManager.Instance.curScene)
            {
                case "MultiPlayTest":
                case "Square":
                    currenctCharacter = await SpawnProcess("Player", new Vector3(0, 2, 0), Quaternion.identity);
                    Debug.Log($"curScene: {SceneManager.Instance.curScene}, spawn Player");
                    break;
                case "MyRoomTest":
                    currenctCharacter = await SpawnProcess("Player");
                    Debug.Log($"curScene: {SceneManager.Instance.curScene}, spawn Player");
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
            GameObject connectionPrefab = await Resources.LoadAsync<GameObject>("Prefabs/" + prefabName) as GameObject;
            return await Runner.SpawnAsync(connectionPrefab, position, rotation);
        }

        public void PlayerLeft(PlayerRef player)
        {
            hasSceneAuthority = Runner.IsSceneAuthority;
        }
    }
}