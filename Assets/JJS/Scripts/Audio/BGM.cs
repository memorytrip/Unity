using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip[] bgmList;
    private AudioSource _bgmPlayer;

    private void Awake()
    {
        _bgmPlayer = GetComponent<AudioSource>();
        _bgmPlayer.loop = true;
        _bgmPlayer.playOnAwake = true;
        SceneManager.sceneLoaded += ChangeBGM;
        SceneManager.sceneUnloaded += StopDuringTransition;
    }

    private void ChangeBGM(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Login":
                PlayBGM(EBgmList.Intro);
                break;
            case "Square":
                PlayBGM(EBgmList.Lobby);
                break;
            default:
                _bgmPlayer.clip = null;
                break;
        }
    }
    
    public void PlayBGM(EBgmList bgmNumber)
    {
        StopBGM();
        _bgmPlayer.clip = bgmList[(int)bgmNumber];
        _bgmPlayer.Play();
    }

    public void StopDuringTransition(Scene scene)
    {
        if (scene.name == "Login" || scene.name == "Square")
        {
            StopBGM();
        }
    }
    
    public void StopBGM()
    {
        if (_bgmPlayer.isPlaying)
        {
            _bgmPlayer.Stop();
        }
    }

    public bool IsPlaying()
    {
        return _bgmPlayer.isPlaying;
    }

    public void SetVolume(float volume)
    {
        _bgmPlayer.volume = volume;
    }
}