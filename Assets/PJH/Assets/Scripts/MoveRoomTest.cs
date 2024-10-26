using System;
using System.Collections.Generic;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveRoomTest : MonoBehaviour
{
    //[SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button EnterButton;
    //[SerializeField] private UserListControll userList;
    private string UserListTest = "SelectPhotoScene";

    private void Awake()
    {
        EnterButton.onClick.AddListener(() => Enter().Forget());
    }

    private async UniTaskVoid Enter()
    {
        try
        {
            EnterButton.interactable = false;
            await SceneManager.Instance.MoveRoom(UserListTest);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }
    
}
