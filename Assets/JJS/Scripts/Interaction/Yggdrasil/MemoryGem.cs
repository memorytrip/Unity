using System;
using Common;
using Cysharp.Threading.Tasks;
using GUI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class MemoryGem : MonoBehaviour, IClickable3dObject
{
    // [SerializeField] private CanvasGroup canvasGroup;
    private VideoPlayer _videoPlayer;
    [SerializeField] private VideoClip dummyVideo;

    [HideInInspector] public VideoData videoData;

    private void Awake()
    {
        // _videoPlayer = canvasGroup.GetComponent<VideoPlayer>();
    }

    public void OnClick()
    {
        /*if (서버에서 갖고 올 수 있다면)
        {
            서버데이터로 갈아끼우기
        }
        else
        {
            DisplayDummyData();
        }*/
        
        OnClickProcess().Forget();
    }

    private async UniTaskVoid OnClickProcess()
    {
        MemoryGemUI memoryGemUI = MemoryGemUI.Instance;
        videoData = await GetServerData();
        if (videoData != null)
        {
            memoryGemUI.videoData = videoData;
            StartCoroutine(memoryGemUI.PlayVideo());
        }
        else
        {
            StartCoroutine(memoryGemUI.PlayDummyVideo());
        }
    }

    private async UniTask<VideoData> GetServerData()
    {
        string rawVideoData = await DataManager.Get("/api/videos");
        Debug.Log(rawVideoData);
        VideoData[] videos = JsonConvert.DeserializeObject<VideoData[]>(rawVideoData);
        if (videos.Length > 0)
        {
            return videos[Random.Range(0, videos.Length)];
        }
        else
        {
            return null;
        }
    }

    private void DisplayDummyData()
    {
        // _videoPlayer.clip = dummyVideo;
    }
}