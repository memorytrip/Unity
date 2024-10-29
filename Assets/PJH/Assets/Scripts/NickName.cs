using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Nickname : MonoBehaviour
{
    public string value;
    public TMP_InputField inputField;
    
    public void SetNickName()
    {
        value = inputField.text;
        PlayerPrefs.SetString("NickName", value);
    }
    
}