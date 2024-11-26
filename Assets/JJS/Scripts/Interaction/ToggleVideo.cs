using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class ToggleVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private Material _material;
    [SerializeField] private Material[] materials;
    [SerializeField] private VideoClip videoClip;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _videoPlayer = GetComponent<VideoPlayer>();
        if (videoClip == null)
        {
            Debug.LogWarning($"{gameObject}: Video clip is missing!");
        }
    }

    private void Start()
    {
        InitializeVideoPlayer();
        _videoPlayer.started += RequestFadeIn;
    }

    private void OnDestroy()
    {
        _videoPlayer.started -= RequestFadeIn;
    }

    private void RequestFadeIn(VideoPlayer videoPlayer)
    {
        // AudioManager.Instance.OnVideoPlayed();
    }

private void InitializeVideoPlayer()
    {
        _videoPlayer.playOnAwake = false;
        _videoPlayer.isLooping = true;
        _videoPlayer.clip = videoClip;
        _material = materials[0];
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        _material = materials[1];
        StartCoroutine(StartVideoPlayback());
        Debug.Log($"Player entered {gameObject}");
    }

    // * 비디오 재생 상황은 플레이어끼리 공유되지 않음! *
    private IEnumerator StartVideoPlayback()
    {
        _videoPlayer.Prepare();
        yield return _videoPlayer.isPrepared; // TODO: 이게 맞나
        _videoPlayer.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        _material = materials[0];
        StopVideoPlayback();
        // AudioManager.Instance.OnVideoStopped();
        Debug.Log($"Player exited {gameObject}");
    }

    private void StopVideoPlayback()
    {
        _videoPlayer.Stop();
    }
}
