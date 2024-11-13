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
        [SerializeField] private InMyroomMapList maplist;
        [SerializeField] private CanvasGroup mapListPanel;
        [SerializeField] private Button closeMapListButton;

        private void Start()
        {
            returnToSquareButton.onClick.AddListener(ReturnToSquare);
            enterEditModeButton.onClick.AddListener(EnterSetting);
            closeMapListButton.onClick.AddListener(CloseSetting);
        }

        private void ReturnToSquare()
        {
            SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();
        }

        private void EnterSetting()
        {
            Utility.EnablePanel(mapListPanel);
            maplist.RefreshListProcess().Forget();
        }

        private void CloseSetting()
        {
            Utility.DisablePanel(mapListPanel);
        }
    }
}