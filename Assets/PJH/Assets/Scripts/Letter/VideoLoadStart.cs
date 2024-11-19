using UnityEngine;

public class VideoLoadStart : MonoBehaviour
{ 
    private VideoProgressBar videoProgressBar;

    public void Awake()
    {
        videoProgressBar = FindAnyObjectByType<VideoProgressBar>();
    }

    public void VideoLoadButtonClicked()
    {
        Debug.Log("버튼이 눌렸습니다");
        videoProgressBar.isLoading = true;
    }
}
