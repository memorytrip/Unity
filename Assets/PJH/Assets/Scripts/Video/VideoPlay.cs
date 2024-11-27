using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    public VideoPlayer player;
    public CanvasGroup playButton;
    public CanvasGroup resumeButton;
    [SerializeField] private bool isPlaying = false;

    private void Awake()
    {
        isPlaying = false;
    }

    public void VideoPlayControll()
    {
        if (!isPlaying)
        {
            CustomFmodBgmManager.Instance.StopPlayback();
            player.Play();
            isPlaying = true;
        }
        else
        {
            CustomFmodBgmManager.Instance.ResumePlayback();
            player.Pause();
            isPlaying = false;
        }
    }
}
