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

    [HideInInspector] public long videoId;
    [HideInInspector] public string videoUrl;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        likeButton.onClick.AddListener(OnVideoLike);
    }

    public IEnumerator PlayVideo(string url)
    {
        Utility.EnablePanel(canvasGroup);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();
    }

    private void OnVideoLike()
    {
        DataManager.Post($"/api/videos/{videoId}/like").Forget();
    }
}
