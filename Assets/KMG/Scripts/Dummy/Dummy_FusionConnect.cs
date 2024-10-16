using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KMG.Scripts.Dummy
{
    public class Dummy_FusionConnect: MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button connectButton;
        [SerializeField] private Button disconnectButton;

        private void Awake()
        {
            connectButton.onClick.AddListener(Connect);
            disconnectButton.onClick.AddListener(Disconnect);
        }

        private void Connect()
        {
            // RunnerManager.Instance.Connect(inputField.text);
            SceneManager.Instance.MoveRoom(inputField.text).Forget();
        }

        private void Disconnect()
        {
            RunnerManager.Instance.Disconnect().Forget();
        }
    }
}