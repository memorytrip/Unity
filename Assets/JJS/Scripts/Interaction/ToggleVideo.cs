using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class ToggleVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    // private ScriptUsageVideoPlayback _scriptUsageVideoPlayback;
    private Material _material;
    [SerializeField] private Material[] materials;
    [SerializeField] private VideoClip videoClip;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _videoPlayer = GetComponent<VideoPlayer>();
        // _scriptUsageVideoPlayback = GetComponent<ScriptUsageVideoPlayback>();
        if (videoClip == null)
        {
            Debug.LogWarning($"{gameObject}: Video clip is missing!");
        }
    }

    private void Start()
    {
        InitializeVideoPlayer();
    }

    /*private void RequestFadeIn(VideoPlayer videoPlayer)
    {
        // AudioManager.Instance.OnVideoPlayed();
    }*/

    private void RequestBgmStop()
    {
        CustomFmodAmbienceManager.Instance.StopPlayback();
        CustomFmodBgmManager.Instance.StopPlayback();
    }

    private void RequestBgm()
    {
        CustomFmodAmbienceManager.Instance.ResumePlayback();
        CustomFmodBgmManager.Instance.ResumePlayback();
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

        if (!other.GetComponent<NetworkObject>().HasStateAuthority)
        {
            return;
        }

        RequestBgmStop();
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
        // _scriptUsageVideoPlayback.StartPlayback();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!other.GetComponent<NetworkObject>().HasStateAuthority)
        {
            return;
        }

        RequestBgm();
        _material = materials[0];
        StopVideoPlayback();
        // AudioManager.Instance.OnVideoStopped();
        Debug.Log($"Player exited {gameObject}");
    }

    private void StopVideoPlayback()
    {
        _videoPlayer.Stop();
        // _scriptUsageVideoPlayback.StopPlayback();
    }
}