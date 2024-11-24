using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class CustomFmodBgmManager : StudioEventEmitter
{
    private EventInstance _eventInstance;
    [SerializeField] private EventReference loginEvent;
    [SerializeField] private EventReference mainEvent;
    [SerializeField] private EventReference myRoomEvent;
    [SerializeField] private EventReference playReadyEvent;
    [SerializeField] private EventReference playEvent;
    [SerializeField] private EventReference yggdrasilEvent;
    private static CustomFmodBgmManager _instance;
    
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
        EventManager.Instance.LoadCompleteMap += ProcessCountdown;
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
                InitializePlay();
                return;
        }

        if (_eventInstance.hasHandle())
        {
            _eventInstance.start();
            Debug.Log("Starting...");
        }
    }

    private void InitializePlay()
    {
        _eventInstance = RuntimeManager.CreateInstance(playEvent);
        Debug.Log("Starting Countdown");
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.PlayStarted, 0);
        RuntimeManager.StudioSystem.getParameterByName(ParameterNameCache.PlayStarted, out float value);
        float data = value;
        Debug.Log($"EventInstance: {data}");
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.LetterFoundCount, 0);
        RuntimeManager.StudioSystem.getParameterByName(ParameterNameCache.LetterFoundCount, out float value1);
        float data1 = value1;
        Debug.Log($"EventInstance: {data1}");
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.ResultReady, 0);
        RuntimeManager.StudioSystem.getParameterByName(ParameterNameCache.ResultReady, out float value2);
        float data2 = value2;
        Debug.Log($"EventInstance: {data2}");
        _eventInstance.start();
    }

    private void ProcessCountdown()
    {
        StartCoroutine(StartCountdown());
    }
    
    private IEnumerator StartCountdown()
    {
        yield return ParameterNameCache.WaitLonger; // TODO: 로딩 완료됐을 때
        yield return ParameterNameCache.Wait;
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.PlayStarted, 1);
        RuntimeManager.StudioSystem.getParameterByName(ParameterNameCache.PlayStarted, out float value3);
        float data3 = value3;
        Debug.Log($"EventInstance: {data3}");
    }
}