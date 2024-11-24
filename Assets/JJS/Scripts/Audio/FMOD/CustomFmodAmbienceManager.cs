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
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        SceneManager.sceneLoaded += SwitchAmbience;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == SceneName.EmptyScene)
        {
            return;
        }
        _ambienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
        _ambienceEvent.release();
        Debug.Log("WHAT");
    }

    private void SwitchAmbience(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case SceneName.EmptyScene:
                RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.HasPaused, 1);
                _ambienceEvent = RuntimeManager.CreateInstance(mainEvent);
                _ambienceEvent.start();
                Debug.Log("Helloooo 2");
                return;
            case SceneName.Square:
                RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.HasPaused, 0);
                Debug.Log("Helloooo 3");
                break;
            default:
                RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.HasPaused, 0);
                _ambienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
                _ambienceEvent.release();
                break;
        }
    }
}