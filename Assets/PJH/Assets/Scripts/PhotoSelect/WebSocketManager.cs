using UnityEngine;
using WebSocketSharp;
using System;
using Common.Network;

public class WebSocketManager : MonoBehaviour
{
    private static WebSocketManager _instance;
    public static WebSocketManager Instance => _instance;

    private WebSocket _webSocket;
    private string _url = "ws://memorytrip-env.eba-73mrisxy.ap-northeast-2.elasticbeanstalk.com/ws?token=";// 서버 URL
    public bool IsConnected => _webSocket != null && _webSocket.ReadyState == WebSocketState.Open;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    public void StartSocketConnect()
    {
        var realToken = CheckBearer(SessionManager.Instance.currentSession.token);
        var url = _url + realToken;
        Debug.Log("url:"+ url);
        InitializeWebSocket(url);
    }

    private string CheckBearer(string token)
    {
        const string BearerPrefix = "Bearer ";
        
        // "Bearer " 문자열이 토큰 앞에 붙어 있는지 확인
        if (token.StartsWith(BearerPrefix, System.StringComparison.OrdinalIgnoreCase))
        {
            // "Bearer " 부분 제거
            token = token.Substring(BearerPrefix.Length);
        }

        return token;
    }
    
    private void InitializeWebSocket(string url)
    {
        _webSocket = new WebSocket(url);

        //소켓이 연결되어 있을 때 출력
        _webSocket.OnOpen += (sender, e) =>
        {
            Debug.LogWarning("WebSocket connected.");
        }; 

        //서버에서 메시지를 받았을 때 호출
        _webSocket.OnMessage += (sender, e) =>
        {
            Debug.LogWarning($"Message received: {e.Data}");
            HandleMessage(e.Data); // 메시지 처리
        };

        //오류가 발생하면 호출
        _webSocket.OnError += (sender, e) =>
        {
            Debug.LogError($"WebSocket error: {e.Message}");
        };

        //서버와 연결이 끊어지면 출력
        _webSocket.OnClose += (sender, e) =>
        {
            Debug.Log($"WebSocket closed. Code: {e.Code}, Reason: {e.Reason}");
        };

        _webSocket.ConnectAsync(); //소켓 서버에 비동기 연결 시작
    }
    
    private void OnDestroy()
    {
        _webSocket?.Close();
    }

    //내가 서버로 메시지를 보냄 이거는 포스트가 아닌가? 뭐지? 
    public void SendMessage(string message)
    {
        if (IsConnected)
        {
            _webSocket.SendAsync(message, null);
        }
        else
        {
            Debug.LogError("WebSocket is not connected.");
        }
    }

    //받은 메시지를 처리
    private void HandleMessage(string data)
    {
        // 메시지 데이터를 처리하는 로직 추가
        Debug.Log($"Processed message: {data}");
    }
}