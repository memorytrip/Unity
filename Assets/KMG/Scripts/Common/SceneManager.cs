using System;
using System.Collections;
using Common.Network;
using Cysharp.Threading.Tasks;
using GUI;
using UnityEngine;

namespace Common
{
    /**
     * 씬 / Fusion 방 이동을 구현하는 클래스
     * DontDestroyOnLoad
     */
    public class SceneManager: MonoBehaviour
    {
        public static SceneManager Instance = null;
        [HideInInspector] public string curScene;
        [HideInInspector] public event Action OnLoadScene;
        [SerializeField] private FadeController fader;
        
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);
        }

#region MoveRoom
        public async UniTask MoveRoom(string roomName)
        {
            await UniTask.WhenAll(FadeOut().ToUniTask(), MoveRoomProcess("MultiPlayTest"));
            await ChangeSceneWithCheckNetworkRunner("MultiPlayTest").ToUniTask();
            await FadeIn().ToUniTask();
        }

        private async UniTask MoveRoomProcess(string roomName)
        {
            try
            {
                if (RunnerManager.Instance.isRunnerExist)
                    await RunnerManager.Instance.Disconnect();
                await RunnerManager.Instance.Connect(roomName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                // TODO: 예외처리
            }
        }

        private string RoutingScene(string roomName)
        {
            if (roomName == "0") return "Lobby";
            if (roomName.Length == 4) return "Play";
            if (roomName.Contains("player_")) return "MyRoom";
            throw new ArgumentException("try to connect invalid room name");
        }
#endregion

#region MoveScene
        public void MoveScene(string sceneName)
        {
            StartCoroutine(MoveSceneProcess(sceneName));
        }

        private IEnumerator MoveSceneProcess(string sceneName)
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

            try
            {
                OnLoadScene?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
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