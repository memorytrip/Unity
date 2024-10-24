using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class Nickname : MonoBehaviour
{
    public static string Value;
    public TMP_InputField inputField;

    private void OnEnable()
    {
        inputField.text = Value;
        inputField.onValueChanged.AddListener(OnChanged);
    }

    private void OnDisable()
    {
        inputField.onValueChanged.RemoveListener(OnChanged);
    }

    private void OnChanged(string nickname)
    {
        Nickname.Value = nickname;
    }
    
}