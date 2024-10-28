using System;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GUI
{
    public class InMyroom: MonoBehaviour
    {
        [SerializeField] private Button returnToSquareButton;
        [SerializeField] private Button enterEditModeButton;

        private void Start()
        {
            returnToSquareButton.onClick.AddListener(ReturnToSquare);
            enterEditModeButton.onClick.AddListener(() => EnterEditMode().Forget());
        }

        private void ReturnToSquare()
        {
            SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();
        }

        private async UniTaskVoid EnterEditMode()
        {
            await RunnerManager.Instance.Disconnect();
            await SceneManager.Instance.MoveSceneProcess("MapEdit");
        }
    }
}