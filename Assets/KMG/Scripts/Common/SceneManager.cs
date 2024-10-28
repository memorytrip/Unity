using System;
using System.Collections;
using Common.Network;
using Cysharp.Threading.Tasks;
using GUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Common
{
    /**
     * 씬 / Fusion 방 이동을 구현하는 클래스
     * DontDestroyOnLoad
     */
    public class SceneManager: MonoBehaviour
    {
        public static readonly string SquareScene = "0";
        
        public static SceneManager Instance = null;
        [HideInInspector] public string curScene;
        public event Action OnSceneLoaded;
        [SerializeField] private FadeController fader;

        public event Action OnSceneUnloaded;

        public void OnSceneEnded()
        {
            OnSceneUnloaded?.Invoke();
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (s, m) => OnSceneLoaded?.Invoke();
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += (s) => OnSceneUnloaded?.Invoke();
        }

#region MoveRoom
        public async UniTask MoveRoom(string roomName)
        {
            OnSceneEnded(); // 이벤트 -> BGM 페이드아웃
            await UniTask.WhenAll(FadeOut().ToUniTask(), MoveRoomProcess(roomName));
            await ChangeSceneWithCheckNetworkRunner(RoutingScene(roomName)).ToUniTask();
            await FadeIn().ToUniTask();
        }

        private async UniTask MoveRoomProcess(string roomName)
        {
            try
            {
                if (RunnerManager.Instance.isRunnerExist)
                    await RunnerManager.Instance.Disconnect();
                await RunnerManager.Instance.Connect(roomName);
                Debug.Log($"Move to Room ({roomName})");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                // TODO: 예외처리
            }
        }

        private string RoutingScene(string roomName)
        {
            // return "MultiPlayTest";
            if (roomName == "0") return "Square";
            if (roomName.Length == 4) return "PlayReady";
            if (roomName.Contains("player_")) return "MyRoom";
            if (roomName == "1") return "MultiPlayTest";
            throw new ArgumentException("try to connect invalid room name");
        }
#endregion

#region MoveScene
        public void MoveScene(string sceneName)
        {
            StartCoroutine(MoveSceneProcess(sceneName));
        }

        public IEnumerator MoveSceneProcess(string sceneName)
        {
            yield return FadeOut();
            yield return ChangeSceneWithCheckNetworkRunner(sceneName);
            yield return FadeIn();
        }
#endregion
        
        /**
         * Runner가 있으면 Runner.LoadScene을 아니면 UnityEngine의 LoadScene을 실행
         */
        private IEnumerator ChangeSceneWithCheckNetworkRunner(string sceneName)
        {
            curScene = null;
            if (RunnerManager.Instance.isRunnerExist)
            {
                var Runner = RunnerManager.Instance.Runner;
                if (Runner.IsSceneAuthority)
                {
                    yield return Runner.LoadScene(sceneName);
                }
            }
            else
            {
                yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            }
            curScene = sceneName;
        }

        private IEnumerator FadeIn()
        {
            yield return fader.FadeOut(0.5f);
        }
        
        private IEnumerator FadeOut()
        {
            yield return fader.FadeIn(0.5f);
        }
    }
}