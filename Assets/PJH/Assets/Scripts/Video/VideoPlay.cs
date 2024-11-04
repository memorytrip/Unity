using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    public VideoPlayer player;
    public TMP_Text playButton;
    private bool isPlaying = false;
    
    public void VideoPlayControll()
    {
        if (!isPlaying)
        {
            playButton.text = "Pause";
            player.Play();
            isPlaying = true;
        }
        else
        {
            playButton.text = "Play";
            player.Pause();
            isPlaying = false;
        }
    }
}
