using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class VcaVolumeController : MonoBehaviour
{
    private static VcaVolumeController _instance;
    
    [SerializeField] private Button volumeSettingsButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider[] volumeSliders;
    [SerializeField] private string vcaName;
    [SerializeField] private string[] individualVcaNames;
    
    private VCA _mainVca;
    private VCA _ambientVca;
    private VCA _musicVca;
    private VCA _sfxVca;
    private VCA _systemVca;

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
    }
    
    private void Start()
    {
        _mainVca = FMODUnity.RuntimeManager.GetVCA(EventPathCache.VcaPath + vcaName);
        _ambientVca = FMODUnity.RuntimeManager.GetVCA(EventPathCache.VcaPath + individualVcaNames[0]);
        _musicVca = FMODUnity.RuntimeManager.GetVCA(EventPathCache.VcaPath + individualVcaNames[1]);
        _sfxVca = FMODUnity.RuntimeManager.GetVCA(EventPathCache.VcaPath + individualVcaNames[2]);
        _systemVca = FMODUnity.RuntimeManager.GetVCA(EventPathCache.VcaPath + individualVcaNames[3]);
        
        mainVolumeSlider.onValueChanged.AddListener(_ => SetVolume(_mainVca, mainVolumeSlider.value));
        volumeSliders[0].onValueChanged.AddListener(_ => SetVolume(_ambientVca, volumeSliders[0].value));
        volumeSliders[1].onValueChanged.AddListener(_ => SetVolume(_musicVca, volumeSliders[1].value));
        volumeSliders[2].onValueChanged.AddListener(_ => SetVolume(_sfxVca, volumeSliders[2].value));
        volumeSliders[3].onValueChanged.AddListener(_ => SetVolume(_systemVca, volumeSliders[3].value));

        volumeSettingsButton.onClick.AddListener(GetVolume);
    }

    private void GetVolume()
    {
        _mainVca.getVolume(out float mainVolume);
        mainVolumeSlider.value = mainVolume;
        _ambientVca.getVolume(out float ambientVolume);
        _musicVca.getVolume(out float musicVolume);
        _sfxVca.getVolume(out float sfxVolume);
        _systemVca.getVolume(out float systemVolume);
        volumeSliders[0].value = ambientVolume;
        volumeSliders[1].value = musicVolume;
        volumeSliders[2].value = sfxVolume;
        volumeSliders[3].value = systemVolume;
    }

    private void SetVolume(VCA vca, float value)
    {
        vca.setVolume(value);
    }
}