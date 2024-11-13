using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Common.Network;

public class NewChat : MonoBehaviour
{
    public TMP_Text textPrefab;             // 텍스트 프리팹
    public TMP_InputField chatInput;        // 채팅 입력 필드
    private const int maxChatCount = 20;    // 최대 채팅 개수
    private List<TMP_Text> chatMessages = new List<TMP_Text>(); // 채팅 메시지 리스트
    
    public static NewChat Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ChatClear()
    {
        for (int i = 0; i < chatMessages.Count; i++)
        {
            Destroy(chatMessages[i]);
        }
    }
    
    public void ButtonClicked()
    {
        Connection.StateAuthInstance.RpcNewChatMsg(chatInput.text);
        chatInput.text = "";
    }

    public void InstantiateChat(string msg)
    {
        var textInstance = Instantiate(textPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, transform);
        textInstance.transform.localPosition = new Vector3(0f, -100f, 0f);
        textInstance.text = msg; 
        
        chatMessages.Add(textInstance);
        
        if (chatMessages.Count > maxChatCount)
        {
            Destroy(chatMessages[0].gameObject); 
            chatMessages.RemoveAt(0);             
        }
        
        UpdateChatPositions();
    }

    private void UpdateChatPositions()
    {
        for (int i = 0; i < chatMessages.Count; i++)
        {
            chatMessages[i].transform.localPosition += new Vector3(0f, 100f, 0f);
        }
    }
}