using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomFmodSfxManager : StudioEventEmitter
{
    private EventInstance _sfxEvent;
    private EventInstance _oneshotEvent;
    [SerializeField] private EventReference playEvent;
    [SerializeField] private EventReference findPhotoEvent;
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

        SceneManager.sceneLoaded += InitializeSfxManager;
        SceneManager.sceneUnloaded += UnsubscribeFromEvents;
    }

    private void InitializeSfxManager(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case SceneName.EmptyScene:
                _sfxEvent.release();
                break;
            // TODO: 보완 필요
            case SceneName.FindPhoto:
                _sfxEvent = RuntimeManager.CreateInstance(playEvent);
                _oneshotEvent = RuntimeManager.CreateInstance(findPhotoEvent);
                SubscribeToEvents();
                break;
        }
        
        _sfxEvent.start();
    }

    private void StartGame()
    {
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.PlayStarted, 1);
    }

    private void FindPhoto(int count)
    {
        _oneshotEvent.start();
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.LetterFoundCount, count);
    }

    private void SubscribeToEvents()
    {
        EventManager.Instance.LoadCompleteMap += StartGame;
        EventManager.Instance.FindPhoto += FindPhoto;
    }

    private void UnsubscribeFromEvents(Scene scene)
    {
        _sfxEvent.release();
        _oneshotEvent.release();
        EventManager.Instance.LoadCompleteMap -= StartGame;
        EventManager.Instance.FindPhoto -= FindPhoto;
    }
}