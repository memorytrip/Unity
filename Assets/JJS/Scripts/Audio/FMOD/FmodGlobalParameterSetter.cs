using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class FmodGlobalParameterSetter : MonoBehaviour
{
    private Button _mainMenuButton;
    private bool _hasPaused;

    private float value;
    private float finalValue;

    public void TogglePause()
    {
        Debug.Log("?????");
        RuntimeManager.StudioSystem.getParameterByName(ParameterNameCache.HasPaused, out value, out finalValue);
        SetGlobalParameter(ParameterNameCache.HasPaused, (int)finalValue == 1 ? 0 : 1);
    }

    private void SetParameter(string parameterName, float value)
    {
        //_bgmIntroEvent.setParameterByName(parameterName, value);
    }

    private void SetParameter(string parameterName, int value)
    {
        //_bgmIntroEvent.setParameterByName(parameterName, value);
    }

    private void SetParameter(string parameterName, string label, bool ignoreSeekSpeed = false)
    {
        //_bgmIntroEvent.setParameterByNameWithLabel(parameterName, label, ignoreSeekSpeed);
    }

    private void SetGlobalParameter(string parameterName, int parameterValue)
    {
        var result = RuntimeManager.StudioSystem.setParameterByName(parameterName, parameterValue);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError($"Failed to set parameter: {parameterName}, value: {parameterValue}, result: {result}");
        }
        else
        {
            RuntimeManager.StudioSystem.getParameterByName(ParameterNameCache.HasPaused, out finalValue);
            Debug.Log($"Value changed: {finalValue}");
        }
    }
    
    private void SetGlobalParameter(string parameterName, float value)
    {
        var result = RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError($"Failed to set parameter: {parameterName}, value: {value}, result: {result}");
        }
    }
    
    private void SetGlobalParameter(string parameterName, string label, bool ignoreSeekSpeed = false)
    {
        var result = RuntimeManager.StudioSystem.setParameterByNameWithLabel(parameterName, label, ignoreSeekSpeed);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError($"Failed to set parameter: {parameterName}, value: {label}, result: {result}");
        }
    }

    private void OnDestroy()
    {
    }
}