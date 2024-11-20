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
    public bool CanTalk { get; private set; }
    
    public void OnClick()
    {
        if (!CanTalk)
        {
            return;
        }
        
        if (!_isDirty)
        {
            director.playableAsset = cutscenes[(int)Cutscenes.Intro];
            _isDirty = true;
        }
        else
        {
            director.playableAsset = cutscenes[(int)Cutscenes.Default];
        }
        //director.Play(); TODO: 나중에
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanTalk = false;
        }
    }
}