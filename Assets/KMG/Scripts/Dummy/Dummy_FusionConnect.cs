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
        [SerializeField] private Button connectButton;

        private void Awake()
        {
            connectButton.onClick.AddListener(()=>Connect().Forget());
        }

        private async UniTaskVoid Connect()
        {
            // RunnerManager.Instance.Connect(inputField.text);
            try
            {
                connectButton.interactable = false;    
                await SceneManager.Instance.MoveRoom("0");
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