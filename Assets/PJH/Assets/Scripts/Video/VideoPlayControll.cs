using GUI;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    public VideoPlayer player;
    public CanvasGroup playButton;
    public CanvasGroup resumeButton;
    private bool isPlaying = false;
    
    public void VideoPlayControll()
    {
        if (!isPlaying)
        {
            UIManager.HideUI(playButton);
            UIManager.ShowUI(resumeButton);
            player.Play();
            isPlaying = true;
        }
        else
        {
            UIManager.ShowUI(playButton);
            UIManager.HideUI(resumeButton);
            player.Pause();
            isPlaying = false;
        }
    }
}
