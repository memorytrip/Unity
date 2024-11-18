using System;
using System.Collections;
using System.Linq;
using Common.Network;
using Fusion;
using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public TMP_InputField chat;
    public TMP_Text chatDisplay;
    public TMP_Text[] chatLog;

    public static Chat Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ChatButtonClicked()
    {
        Connection.StateAuthInstance.RpcChatMsg(SessionManager.Instance.currentUser.nickName + ": " + chat.text); //닉네임 부분 실제 닉네임 받아서 써야함
        chat.text = string.Empty;
    }

    public void Append(string msg)
    {
        for (int i = chatLog.Length -1; i > 0; i--)
        {
            chatLog[i].text = chatLog[i-1].text;
        }
        chatLog[0].text = msg;
        chatDisplay.text = chatLog[0].text;
    }
}
