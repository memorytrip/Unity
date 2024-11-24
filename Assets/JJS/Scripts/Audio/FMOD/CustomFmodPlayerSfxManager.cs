using FMOD.Studio;
using FMODUnity;
using UnityEngine;
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
            DontDestroyOnLoad(this);
        }
        
        _eventInstance = RuntimeManager.CreateInstance(footstepEvent);
        _eventInstance.start();
    }

    private void Start()
    {
        EventManager.Instance.StartWalking += StartWalking;
        EventManager.Instance.StopWalking += StopWalking;
    }

    private void OnDisable()
    {
        EventManager.Instance.StartWalking -= StartWalking;
        EventManager.Instance.StopWalking -= StopWalking;
        _eventInstance.stop(STOP_MODE.IMMEDIATE);
    }

    private void StartWalking()
    {
        _eventInstance.setParameterByName(ParameterNameCache.IsWalking, 1);
        Debug.Log("Yaaaay");
    }

    private void StopWalking()
    {
        _eventInstance.setParameterByName(ParameterNameCache.IsWalking, 0);
        Debug.Log("Stop!");
    }
}
