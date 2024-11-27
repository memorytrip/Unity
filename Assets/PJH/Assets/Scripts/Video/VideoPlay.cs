using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    public VideoPlayer player;
    public CanvasGroup playButton;
    public CanvasGroup resumeButton;
    [SerializeField] private bool isPlaying = false;
    
    private AudioSource audioSource;

    private void Awake()
    {
        isPlaying = false;
    }

    private void Start()
    {
        InitializeVideoPlayer();
    }
    
    private void InitializeVideoPlayer()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log(audioSource);
        player.playOnAwake = false;
        player.isLooping = true;
        player.audioOutputMode = VideoAudioOutputMode.AudioSource;
        player.SetTargetAudioSource(0, audioSource);
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

    public void Play()
    {
        if (player.isPlaying) 
            return;
        CustomFmodAmbienceManager.Instance.StopPlayback();
        CustomFmodBgmManager.Instance.StopPlayback();
        player.Play();
    }
    
    public void Pause()
    {
        if (!player.isPlaying) 
            return;
        CustomFmodAmbienceManager.Instance.ResumePlayback();
        CustomFmodBgmManager.Instance.ResumePlayback();
        player.Pause();
    }
}
