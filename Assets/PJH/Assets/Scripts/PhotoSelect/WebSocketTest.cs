using System;
using System.Collections.Generic;
using Common.Network;
using Fusion;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

public class SelectedPhotoRequest
{
    public string roomCode;
    public string text;
    public List<string> photoUrl;
}

public class VideoData
{
    [JsonProperty("videoId")]
    public string VideoId { get; set; }

    [JsonProperty("videoData")]
    public string videoData { get; set; }
}

public class WebSocketTest : MonoBehaviour
{
    private WebSocket ws;
    public string uri = "http://memorytrip-env.eba-73mrisxy.ap-northeast-2.elasticbeanstalk.com/api/videos/request-video";
    public List<string> photoIdList = new List<string>();
    public LoadLetterAndPhoto loadLP;
    private Texture2D videoTexture;
    [SerializeField] private Renderer videoRenderer; //video 컴포넌트에 넣기

    private void Start()
    {
        for (int i = 0; i <loadLP.photoButtons.Length; i++)
        {
            int index = i;
            loadLP.photoButtons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
        
        ws = new WebSocket(uri);

        ws.OnMessage += OnMessageReceieved;
        ws.Connect();
    }
    
    void OnButtonClicked(int index)
    {
        SendSelectedPhotoIds(photoIdList, index);
    }

    public void SendSelectedPhotoIds(List<string> selectedPhotoIds, int index)
    {
        SelectedPhotoRequest req = new SelectedPhotoRequest
        {
            roomCode = RunnerManager.Instance.Runner.SessionInfo.Name,
            //photoUrl = SessionManager.Instance.currentUser.email,
            //photoIds = photoIdList
        };

        string jsonReq = JsonConvert.ToString(req);

        ws.Send(jsonReq);
    }

    private void OnMessageReceieved(object sender, MessageEventArgs e)
    {
        Debug.Log("성공");

        try
        {
            VideoData videoData = JsonConvert.DeserializeObject<VideoData>(e.Data);
            byte[] videoBytes = Convert.FromBase64String(videoData.videoData);
            LoadVideoTexture(videoBytes);
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to process video data: {exception.Message}");
        }
    }
    
    private void LoadVideoTexture(byte[] videoBytes)
    {
        // 임시 텍스처 생성
        if (videoTexture == null)
            videoTexture = new Texture2D(2, 2);

        // 바이너리 데이터를 텍스처로 디코딩
        if (videoTexture.LoadImage(videoBytes))
        {
            Debug.Log("Video texture successfully loaded.");
            videoRenderer.material.mainTexture = videoTexture; // 렌더러에 텍스처 적용
        }
        else
        {
            Debug.LogError("Failed to load texture from video data.");
        }
    }
}
