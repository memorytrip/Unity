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
            await UniTask.WhenAll(FadeOut().ToUniTask(), MoveRoomProcess("SampleScene"));
            await ChangeSceneWithCheckNetworkRunner("SampleScene").ToUniTask();
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