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
    private bool isPlaying = false;
    [SerializeField] private Button returnButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(ReturnToSquare);
    }

    private void ReturnToSquare() => SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();

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
