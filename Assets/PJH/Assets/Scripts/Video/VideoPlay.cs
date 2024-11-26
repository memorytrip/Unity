using System;
using Common;
using Cysharp.Threading.Tasks;
using GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
