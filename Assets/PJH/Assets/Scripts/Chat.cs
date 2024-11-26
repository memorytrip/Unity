using Common.Network;
using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public TMP_InputField chat;
    public TMP_Text[] chatLog;
    [SerializeField] private TextMeshProUGUI recentChatText;

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
        for (int i = 0; i < chatLog.Length - 1; i++)
        {
            chatLog[i].text = chatLog[i + 1].text;
        }
    
        recentChatText.text = msg;
        chatLog[^1].text = msg;
        
        /*if (!chatLog[0].text.IsNullOrEmpty())
        {
            for (int i = 0; i < chatLog.Length - 1; i++)
            {
                chatLog[i].text = chatLog[i + 1].text;
            }
            chatLog[^1].text = msg;
        }
        else
        {
            chatLog[0].text = msg;
        }*/
        /*for (int i = chatLog.Length -1; i > 0; i--)
        {
            chatLog[i].text = chatLog[i-1].text;
        }
        chatLog[0].text = msg;
        //chatDisplay.text = chatLog[0].text;*/
    }
}