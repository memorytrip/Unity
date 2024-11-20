using UnityEngine;
using UnityEngine.Playables;

public class Standby : MonoBehaviour
{
    private PlayableDirector _director;
    private const string LoginSceneName = "Login";

    [SerializeField] private PlayableAsset introTransition;
    
    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        if (SceneTracker.PreviousScene == LoginSceneName)
        {
            StartCutscene(introTransition);
        }
    }
    
    private void StartCutscene(PlayableAsset cutscene)
    {
        if (_director.state == PlayState.Playing)
        {
            _director.Stop();
        }
        _director.Play(cutscene);
    }
}
