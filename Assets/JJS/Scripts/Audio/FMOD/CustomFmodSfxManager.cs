using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
        }

        SceneManager.sceneLoaded += InitializeSfxManager;
    }

    private new void Start()
    {
        SubscribeToEvents();
    }

    private new void OnDestroy()
    {
        if (_instance == this)
        {
            UnsubscribeFromEvents();
        }
    }

    private void InitializeSfxManager(Scene scene, LoadSceneMode mode)
    {
        if (_sfxEvent.isValid())
        {
            _sfxEvent.stop(STOP_MODE.IMMEDIATE);
            _sfxEvent.release();
        }
        
        switch (scene.name)
        {
            // TODO: 보완 필요
            case SceneName.FindPhoto:
                _sfxEvent = RuntimeManager.CreateInstance(playEvent);
                _oneshotEvent = RuntimeManager.CreateInstance(findPhotoEvent);
                _oneshotEvent1 = RuntimeManager.CreateInstance(tadaEvent);
                break;
        }

        if (_sfxEvent.isValid())
        {
            _sfxEvent.start();
        }
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
        if (_oneshotEvent.isValid())
        {
            _oneshotEvent.start();
        }
        yield return _wait;
        if (_oneshotEvent1.isValid())
        {
            _oneshotEvent1.start();
        }
    }

    private void SubscribeToEvents()
    {
        EventManager.Instance.LoadCompleteMap += StartGame;
        EventManager.Instance.FindPhoto += FindPhoto;
    }

    private void UnsubscribeFromEvents()
    {
        EventManager.Instance.LoadCompleteMap -= StartGame;
        EventManager.Instance.FindPhoto -= FindPhoto;

        if (_sfxEvent.isValid())
        {
            _sfxEvent.release();
        }

        if (_oneshotEvent.isValid())
        {
            _oneshotEvent.release();
        }

        if (_oneshotEvent1.isValid())
        {
            _oneshotEvent1.release();
        }
    }
}