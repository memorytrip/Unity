using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using Common.Network;
using Cysharp.Threading.Tasks;
using UnityEngine.Video;

public class WebSocketManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private static WebSocketManager _instance;
    public static WebSocketManager Instance => _instance;
    private CancellationTokenSource _cancellationTokenSource;

    private WebSocket _webSocket;
    private string _url = "125.132.216.190:17778/ws?token=";// 서버 URL
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
            KeepConnectionAlive().Forget();
        }; 

        //서버에서 메시지를 받았을 때 호출
        _webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("소켓 정보가 넘어옴:" + e.Data);
            HandleIncomingVideoData(e.Data);
        };

        //오류가 발생하면 호출
        _webSocket.OnError += (sender, e) =>
        {
            Debug.LogError($"WebSocket error: {e.Message}");
        };

        //서버와 연결이 끊어지면 출력
        _webSocket.OnClose += (sender, e) =>
        {
            Debug.LogWarning($"WebSocket closed. Code: {e.Code}, Reason: {e.Reason}");
            StartCoroutine(ReconnectWebSocket(url));
        };

        _webSocket.ConnectAsync(); //소켓 서버에 비동기 연결 시작
    }
    
    private void OnDestroy()
    {
        _webSocket?.Close();
        _cancellationTokenSource?.Cancel();
    }

    private async UniTask KeepConnectionAlive()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        while (IsConnected)
        {
            try
            {
                _webSocket.Send("ping");
                Debug.Log("핑!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ping 전송 중 오류 발생: {ex.Message}");
                break; // 에러 발생 시 반복문 종료
            }

            // 30초 대기
            await UniTask.Delay(TimeSpan.FromSeconds(30), cancellationToken: token);
        }

        Debug.LogWarning("KeepConnectionAlive가 종료되었습니다.");
    }

    
    private void HandleIncomingVideoData(string videoUrl)
    {
        if (string.IsNullOrEmpty(videoUrl))
        {
            Debug.LogWarning("비디오 정보 없음");
            return;
        }

        Debug.Log("Url:" + videoUrl);

        videoPlayer.url = videoUrl;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.Play();
    }
    
    private IEnumerator ReconnectWebSocket(string url)
    {
        const int maxRetryAttempts = 5; // 최대 재시도 횟수
        const float retryInterval = 5.0f; // 재시도 간격 (초)
        int attempt = 0;

        while (attempt < maxRetryAttempts && (_webSocket == null || _webSocket.ReadyState != WebSocketState.Open))
        {
            attempt++;
            Debug.Log($"WebSocket 재연결 시도 {attempt}/{maxRetryAttempts}...");
            _webSocket = null;
            InitializeWebSocket(url);

            // 연결 상태 확인
            yield return new WaitForSeconds(retryInterval);
        }

        if (_webSocket != null && _webSocket.ReadyState == WebSocketState.Open)
        {
            Debug.Log("WebSocket 재연결 성공.");
        }
        else
        {
            Debug.LogError("WebSocket 재연결 실패.");
        }
    }

    private void SaveAndPlayVideo(byte[] videoData)
    {
        // 1. 임시 파일 경로 지정
        string filePath = Path.Combine(Application.persistentDataPath, "temp_video.mp4");

        // 2. 영상 데이터를 파일로 저장
        File.WriteAllBytes(filePath, videoData);
        Debug.Log($"Video saved at: {filePath}");

        // 3. VideoPlayer에 파일 경로 연결
        PlayVideo(filePath);
    }

    private void PlayVideo(string videoPath)
    {
        // VideoPlayer 설정
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();
    }
    
    //받은 메시지를 처리
    private void HandleMessage(string data)
    {
        // 메시지 데이터를 처리하는 로직 추가
        Debug.Log($"Processed message: {data}");
    }
}