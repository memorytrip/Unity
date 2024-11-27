using Fusion;
using TMPro;
using UnityEngine;


public class EmotionInput : NetworkBehaviour
{
    public TMP_InputField emotionInput;
    
    public string EmotionText { get; set; }
    
    public void OnInputChanged()
    {
        if (HasStateAuthority)
        {
            EmotionText = emotionInput.text;
        }
    }
    
    public override void FixedUpdateNetwork()
    {
        OnInputChanged();
        if (emotionInput != null && emotionInput.text != EmotionText)
        {
            emotionInput.text = EmotionText;
        }
    }
    
}