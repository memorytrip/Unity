using System;
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
            connectButton.onClick.AddListener(()=>Connect().Forget());
            disconnectButton.onClick.AddListener(Disconnect);
        }

        private async UniTaskVoid Connect()
        {
            // RunnerManager.Instance.Connect(inputField.text);
            try
            {
                connectButton.interactable = false;    
                await SceneManager.Instance.MoveRoom(inputField.text);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                if (connectButton != null)
                    connectButton.interactable = true;    
            }
        }

        private void Disconnect()
        {
            RunnerManager.Instance.Disconnect().Forget();
        }
    }
}