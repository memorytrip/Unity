using Common;
using Common.Network;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
            StartCoroutine(SceneManager.Instance.MoveRoom(inputField.text));
        }

        private void Disconnect()
        {
            RunnerManager.Instance.Disconnect().Forget();
        }
    }
}