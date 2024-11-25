using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class CustomFmodAmbienceManager : StudioEventEmitter
{
    private EventInstance _ambienceEvent;
    [SerializeField] private EventReference mainEvent;
    private static CustomFmodAmbienceManager _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
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

    private void SwitchAmbience(Scene scene, LoadSceneMode mode)
    {
        if (_ambienceEvent.isValid())
        {
            _ambienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
            _ambienceEvent.release();
        }
        
        switch (scene.name)
        {
            case SceneName.EmptyScene:
                _ambienceEvent = RuntimeManager.CreateInstance(mainEvent);
                if (_ambienceEvent.isValid())
                {
                    _ambienceEvent.start();
                    Debug.Log("Helloooo 2");
                }
                return;
            case SceneName.Square:
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