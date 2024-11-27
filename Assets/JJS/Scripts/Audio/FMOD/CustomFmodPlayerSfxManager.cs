using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class CustomFmodPlayerSfxManager : MonoBehaviour
{
    private static CustomFmodPlayerSfxManager _instance;
    private EventInstance _eventInstance;
    [SerializeField] private EventReference footstepEvent;

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
        
        _eventInstance = RuntimeManager.CreateInstance(footstepEvent);
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.IsWalking, 0);
        _eventInstance.start();

        SceneManager.sceneLoaded += StopWalkingEvent;
    }

    private void Start()
    {
        EventManager.Instance.StartWalking += StartWalking;
        EventManager.Instance.StopWalking += StopWalking;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= StopWalkingEvent;
        EventManager.Instance.StartWalking -= StartWalking;
        EventManager.Instance.StopWalking -= StopWalking;
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.IsWalking, 0);
        _eventInstance.stop(STOP_MODE.IMMEDIATE);
    }

    private void StartWalking()
    {
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.IsWalking, 1);
        Debug.Log("Yaaaay");
    }

    private void StopWalkingEvent(Scene scene, LoadSceneMode mode)
    {
        StopWalking();
    }

    private void StopWalking()
    {
        RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.IsWalking, 0);
        Debug.Log("Stop!");
    }
}
