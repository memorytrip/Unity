using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class EnterMyroom: MonoBehaviour
    {
        [SerializeField] private Button enterButton;
        [SerializeField] private FadeController fader;

        private void Awake()
        {
            enterButton.onClick.AddListener(Enter);
        }

        private void Enter()
        {
            User user = SessionManager.Instance.currentSession?.user;
            if (user != null)
                SceneManager.Instance.MoveRoom($"player_{user.nickName}").Forget();
            // else
            //     SceneManager.Instance.MoveRoom($"player_").Forget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            if (!other.GetComponent<NetworkObject>().HasStateAuthority)
                return;
            fader.StartFadeIn(0.5f);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            if (!other.GetComponent<NetworkObject>().HasStateAuthority)
                return;
            fader.StartFadeOut(0.5f);
        }
    }
}