using System.Collections;
using Common.Network;
using Cysharp.Threading.Tasks;
using GUI;
using UnityEngine;

namespace Common
{
    /**
     * TODO: MoveRoom 구현하기
     */
    public class SceneManager: MonoBehaviour
    {
        public static SceneManager Instance = null;
        public string curScene;
        [SerializeField] private FadeController fader;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator MoveRoom(string roomName)
        {
            if (RunnerManager.Instance.isRunnerExist)
                yield return RunnerManager.Instance.Disconnect();
            yield return RunnerManager.Instance.Connect(roomName);
            MoveScene("SampleScene");
        }
        
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
        }

        private IEnumerator FadeIn()
        {
            yield return fader.FadeOut(1f);
        }
        
        private IEnumerator FadeOut()
        {
            yield return fader.FadeIn(1f);
        }
    }
}