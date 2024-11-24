using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class CustomFmodAmbienceManager : StudioEventEmitter
{
    private EventInstance _eventInstance;
    [SerializeField] private EventReference loginEvent;
    [SerializeField] private EventReference mainEvent;
    [SerializeField] private EventReference myRoomEvent;
    [SerializeField] private EventReference playReadyEvent;
    [SerializeField] private EventReference playEvent;
    [SerializeField] private EventReference yggdrasilEvent;
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

        SceneManager.sceneLoaded += ChangeBgm;
    }

    private void ChangeBgm(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case SceneName.EmptyScene:
                _eventInstance.release();
                _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
                return;
            // TODO: 보완 필요
            case SceneName.Login:
                _eventInstance = RuntimeManager.CreateInstance(loginEvent);
                break;
            case SceneName.Square:
                _eventInstance = RuntimeManager.CreateInstance(mainEvent);
                break;
            case SceneName.MyRoom:
            case SceneName.PlayReady:
                _eventInstance = RuntimeManager.CreateInstance(myRoomEvent);
                break;
            case SceneName.FindPhoto:
                _eventInstance = RuntimeManager.CreateInstance(playEvent);
                return;
        }
        _eventInstance.start();
    }
}