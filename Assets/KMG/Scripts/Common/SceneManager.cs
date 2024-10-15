using System.Collections;
using UnityEngine;

namespace Common
{
    /**
     * TODO: FadeIn이랑 FadeOut 구현하기
     */
    public class SceneManager: MonoBehaviour
    {
        public static SceneManager Instance = null;
        public string curScene;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        public void MoveScene(string sceneName)
        {
            StartCoroutine(MoveSceneProcess(sceneName));
        }

        public void MoveRoom(string roomName)
        {
            
        }

        public IEnumerator MoveSceneProcess(string sceneName)
        {
            yield break;
        }

        private IEnumerator FadeIn()
        {
            yield break;   
        }
        
        private IEnumerator FadeOut()
        {
            yield break;   
        }
    }
}