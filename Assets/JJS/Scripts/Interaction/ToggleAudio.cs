using UnityEngine;

// TODO: Spatial Audio 하고 싶다ㅏㅏㅏㅏㅏㅏ
[RequireComponent(typeof(AudioSource))]
public class ToggleAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (audioClip == null)
        {
            Debug.LogWarning($"{gameObject}: Audio clip is missing!");
        }
    }

    private void Start()
    {
        InitializeAudioPlayer();
    }
    
    private void InitializeAudioPlayer()
    {
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        _audioSource.clip = audioClip;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        StartAudioPlayback();
        Debug.Log($"Player entered {gameObject}: AudioClip {audioClip}");
    }

    private void StartAudioPlayback()
    {
        _audioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        StopAudioPlayback();
        Debug.Log($"Player exited {gameObject}");
    }
    
    private void StopAudioPlayback()
    {
        _audioSource.Stop();
    }
}
