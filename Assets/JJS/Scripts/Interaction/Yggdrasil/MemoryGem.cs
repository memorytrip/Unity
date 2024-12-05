using System;
using GUI;
using UnityEngine;
using UnityEngine.Video;

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
        MemoryGemUI memoryGemUI = MemoryGemUI.Instance;
        memoryGemUI.videoData = videoData;
        StartCoroutine(memoryGemUI.PlayVideo());
    }

    private void DisplayServerData()
    {
        throw new NotImplementedException();
    }

    private void DisplayDummyData()
    {
        _videoPlayer.clip = dummyVideo;
    }
}