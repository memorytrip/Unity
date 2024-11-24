using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class VcaVolumeController : MonoBehaviour
{
    [SerializeField] private Button volumeSettingsButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider[] volumeSliders;
    [SerializeField] private string vcaName;
    [SerializeField] private string[] individualVcaNames;
    
    private VCA _mainVca;
    private VCA[] _vcaList;

    private void Awake()
    {
        mainVolumeSlider.onValueChanged.AddListener(_ => SetVolume(_mainVca, mainVolumeSlider.value));
        
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            var i1 = i;
            volumeSliders[i].onValueChanged.AddListener(_ => SetVolume(_vcaList[i1], volumeSliders[i1].value));
        }

        volumeSettingsButton.onClick.AddListener(GetVolume);
    }
    
    private void Start()
    {
        _mainVca = FMODUnity.RuntimeManager.GetVCA(EventPathCache.VcaPath + vcaName);
        for (int i = 0; i < individualVcaNames.Length; i++)
        {
            _vcaList[i] = FMODUnity.RuntimeManager.GetVCA(EventPathCache.VcaPath + individualVcaNames[i]);
        }
    }

    private void GetVolume()
    {
        _mainVca.getVolume(out float mainVolume);
        mainVolumeSlider.value = mainVolume;
        for (int i = 0; i < _vcaList.Length; i++)
        {
            _vcaList[i].getVolume(out float vcaVolume);
            volumeSliders[i].value = vcaVolume;
        }
    }

    private void SetVolume(VCA vca, float value)
    {
        vca.setVolume(value);
    }
}