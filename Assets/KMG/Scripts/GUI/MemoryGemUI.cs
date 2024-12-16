using System.Collections;
using Common;
using Cysharp.Threading.Tasks;
using GUI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MemoryGemUI : MonoBehaviour
{
    public static MemoryGemUI Instance;
    
    private CanvasGroup canvasGroup;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button imageButton;
    [SerializeField] private Button likeButton;
    [SerializeField] private VideoClip dummyVideo;

    [HideInInspector] public VideoData videoData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        likeButton.onClick.AddListener(OnVideoLike);
    }

    public IEnumerator PlayVideo()
    {
        Utility.EnablePanel(canvasGroup);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoData.videoUrl;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();
    }

    public IEnumerator PlayDummyVideo()
    {
        Utility.EnablePanel(canvasGroup);
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = dummyVideo;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();
    }

    private void OnVideoLike()
    {
        VideoLikeProcess().Forget();    
    }

    private async UniTaskVoid VideoLikeProcess()
    {
        try
        {
            await DataManager.Post($"/api/videos/{videoData.videoId}/like");
        }
        catch (UnityWebRequestException e)
        {
            PopupManager.Instance.ShowMessage(e);
        }
    }
}