using UnityEngine;
using UnityEngine.Playables;

public class TriggerDialogue : MonoBehaviour, IClickable3dObject
{
    private enum Cutscenes
    {
        Intro,
        Default
    }
    
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset[] cutscenes;
    private bool _isDirty;
    
    public void OnClick()
    {
        if (!_isDirty)
        {
            director.playableAsset = cutscenes[(int)Cutscenes.Intro];
            _isDirty = true;
        }
        else
        {
            director.playableAsset = cutscenes[(int)Cutscenes.Default];
        }
        director.Play();
    }
}
