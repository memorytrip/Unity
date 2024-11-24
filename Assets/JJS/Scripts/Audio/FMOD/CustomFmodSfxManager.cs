using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomFmodSfxManager : StudioEventEmitter
{
    private EventInstance _sfxEvent;
    private EventInstance _oneshotEvent;
    private EventInstance _oneshotEvent1;
    [SerializeField] private EventReference playEvent;
    [SerializeField] private EventReference findPhotoEvent;
    [SerializeField] private EventReference tadaEvent;
    private static CustomFmodSfxManager _instance;
    private readonly WaitForSeconds _wait = new(1f);
    
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
                _oneshotEvent1 = RuntimeManager.CreateInstance(tadaEvent);
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
        StartCoroutine(ProcessFindPhoto(count));
    }

    private IEnumerator ProcessFindPhoto(int count)
    {
        _oneshotEvent.start();
        //RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.LetterFoundCount, count);
        yield return _wait;
        _oneshotEvent1.start();
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