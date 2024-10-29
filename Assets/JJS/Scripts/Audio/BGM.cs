using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

[RequireComponent(typeof(AudioSource))]
public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip[] bgmList;
    private AudioSource _bgmPlayer;
    private Scene _loadedScene;

    private void Awake()
    {
        _bgmPlayer = GetComponent<AudioSource>();
        _bgmPlayer.loop = true;
        _bgmPlayer.playOnAwake = true;
        SceneManager.sceneLoaded += ChangeBGM;
        SceneManager.sceneUnloaded += StopDuringTransition;
        _loadedScene = SceneManager.GetActiveScene();
    }

    private bool IsTransitioning(Scene scene)
    {
        if (_loadedScene != scene)
        {
            return true;
        }

        return false;
    }

    private void ChangeBGM(Scene scene, LoadSceneMode mode)
    {
        _loadedScene = scene;
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
        _bgmPlayer.clip = bgmList[(int)bgmNumber];
        _bgmPlayer.volume = AudioManager.Instance.bgmVolume;
        _bgmPlayer.Play();
    }

    public void StopDuringTransition(Scene scene)
    {
        StopBGM();
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