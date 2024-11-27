using UnityEngine;
using UnityEngine.Video;

public class FmodVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private AudioSource _audioSource;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _videoPlayer.playOnAwake = false;
        _audioSource.playOnAwake = false;
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.SetTargetAudioSource(0, _audioSource);
        _audioSource.outputAudioMixerGroup = null;
    }
}
