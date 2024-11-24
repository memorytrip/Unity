using System;
using Common;
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

        private void Awake()
        {
            connectButton.onClick.AddListener(() => Connect().Forget());
            connectButton.onClick.AddListener(NewChat.Instance.ChatClear);
        }

        private async UniTaskVoid Connect()
        {
            // RunnerManager.Instance.Connect(inputField.text);
            try
            {
                connectButton.interactable = false;
                PlayerPrefs.SetString("NickName", inputField.text);
                await SceneManager.Instance.MoveRoom(SceneName.Square);
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
    }
}