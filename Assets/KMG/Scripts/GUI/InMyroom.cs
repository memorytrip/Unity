using System;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class InMyroom: MonoBehaviour
    {
        [SerializeField] private Button returnToSquareButton;

        private void Start()
        {
            returnToSquareButton.onClick.AddListener(ReturnToSquare);
        }

        private void ReturnToSquare()
        {
            SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();
        }
    }
}