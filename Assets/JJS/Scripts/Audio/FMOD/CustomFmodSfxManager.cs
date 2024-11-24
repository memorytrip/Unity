using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class CustomFmodSfxManager : StudioEventEmitter
{
    private EventInstance _eventInstance;
    [SerializeField] private EventReference loginEvent;
    [SerializeField] private EventReference mainEvent;
    [SerializeField] private EventReference myRoomEvent;
    [SerializeField] private EventReference playReadyEvent;
    [SerializeField] private EventReference playEvent;
    [SerializeField] private EventReference yggdrasilEvent;
    private static CustomFmodSfxManager _instance;
    
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

        SceneManager.sceneLoaded += ChangeBgm;
    }

    private void ChangeBgm(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case SceneName.EmptyScene:
                _eventInstance.release();
                _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
                break;
            // TODO: 보완 필요
            case SceneName.FindPhoto:
                _eventInstance = RuntimeManager.CreateInstance(playEvent);
                break;
        }
        
        _eventInstance.start();
    }
}