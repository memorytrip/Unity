using Common;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider ambienceVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider systemVolumeSlider;

    private void Awake()
    {
        ambienceVolumeSlider.value = AudioManager.Instance.ambienceVolume;
        bgmVolumeSlider.value = AudioManager.Instance.bgmVolume;
        sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
        systemVolumeSlider.value = AudioManager.Instance.systemVolume;

        ambienceVolumeSlider.onValueChanged.AddListener(OnAmbienceVolumeChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        systemVolumeSlider.onValueChanged.AddListener(OnSystemVolumeChanged);
    }

    private void OnAmbienceVolumeChanged(float value)
    {
        AudioManager.Instance.SetAmbienceVolume(value);
    }

    private void OnBgmVolumeChanged(float value)
    {
        AudioManager.Instance.SetBgmVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }

    private void OnSystemVolumeChanged(float value)
    {
        AudioManager.Instance.SetSystemVolume(value);
    }

    private void OnDestroy()
    {
        ambienceVolumeSlider.onValueChanged.RemoveListener(OnAmbienceVolumeChanged);
        bgmVolumeSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        systemVolumeSlider.onValueChanged.RemoveListener(OnSystemVolumeChanged);
    }
}
