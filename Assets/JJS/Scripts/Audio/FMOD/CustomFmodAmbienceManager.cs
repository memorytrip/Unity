using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class CustomFmodAmbienceManager : StudioEventEmitter
{
    private EventInstance _ambienceEvent;
    [SerializeField] private EventReference mainEvent;
    public static CustomFmodAmbienceManager Instance;

    /*private bool _isPlaying(EventInstance eventInstance)
    {
        PLAYBACK_STATE state;
        eventInstance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.STOPPED;
    }*/
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        SceneManager.sceneLoaded += SwitchAmbience;
    }

    private new void OnDestroy()
    {
        SceneManager.sceneLoaded -= SwitchAmbience;
        if (_ambienceEvent.isValid())
        {
            _ambienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
            _ambienceEvent.release();
        }
    }

    public void ResumePlayback()
    {
        if (!_ambienceEvent.isValid())
        {
            return;
        }

        _ambienceEvent.start();
    }
    
    public void StopPlayback()
    {
        if (!_ambienceEvent.isValid())
        {
            return;
        }

        _ambienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void SwitchAmbience(Scene scene, LoadSceneMode mode)
    {
        if (_ambienceEvent.isValid())
        {
            _ambienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
            _ambienceEvent.release();
        }
        
        switch (scene.name)
        {
            case SceneName.Square:
                _ambienceEvent = RuntimeManager.CreateInstance(mainEvent);
                if (_ambienceEvent.isValid())
                {
                    _ambienceEvent.start();
                }
                break;
            default:
                if (_ambienceEvent.isValid())
                {
                    _ambienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
                    _ambienceEvent.release();
                }
                break;
        }
    }
}