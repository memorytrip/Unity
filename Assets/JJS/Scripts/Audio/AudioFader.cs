using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = Common.SceneManager;

public class AudioFader : MonoBehaviour
{
    private AudioSource _audioSource;
    private const float MinVolume = 0f;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SceneManager.Instance.EndScene += FadeIn;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += ResumeBGM;
        AudioManager.Instance.PlayVideo += FadeIn;
        AudioManager.Instance.StopVideo += FadeOut;
    }

    private void OnDestroy()
    {
        SceneManager.Instance.EndScene -= FadeIn;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= ResumeBGM;
        AudioManager.Instance.PlayVideo -= FadeIn;
        AudioManager.Instance.StopVideo -= FadeOut;
    }

    private void FadeIn()
    {
        StartCoroutine(ProcessFade(true, _audioSource, 1f, MinVolume));
    }

    private void FadeOut()
    {
        StartCoroutine(ProcessFade(true, _audioSource, 1f, AudioManager.Instance.bgmVolume));
    }

    private void ResumeBGM(Scene scene, LoadSceneMode mode)
    {
        _audioSource.volume = AudioManager.Instance.bgmVolume;
    }
    
    private IEnumerator ProcessFade(bool startNow, AudioSource source, float duration, float targetVolume)
    {
        if (!startNow)
        {
            double sourceLength = (double)source.clip.samples / source.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(sourceLength - duration));
        }

        float time = 0f;
        float startVolume = source.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        
        Debug.Log($"Volume: {source.volume}");
    }
}
