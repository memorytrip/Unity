using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DummyLoginUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    private readonly WaitForSeconds _wait = new WaitForSeconds(1f);
    private readonly WaitForSeconds _typeDelay = new WaitForSeconds(0.05f);
    [SerializeField] private Button triggerButton;
    [SerializeField] private Button triggerClearButton;
    private const string IdKey = "Traver";
    private const string PasswordKey = "Pass!1234"; // 기절할 비밀번호 조합

    private void Awake()
    {
        triggerButton.onClick.AddListener(StartInput);
        triggerClearButton.onClick.AddListener(ClearInput);
        idInputField.caretWidth = passwordInputField.caretWidth = 2;
        idInputField.caretBlinkRate = passwordInputField.caretBlinkRate = 2f;
        idInputField.caretColor = passwordInputField.caretColor = idInputField.colors.normalColor;
    }
    
    public void StartInput()
    {
        StartCoroutine(AutomateInput());
    }
    
    private IEnumerator AutomateInput()
    {
        yield return _wait;
        idInputField.ActivateInputField();
        foreach (var character in IdKey.ToCharArray())
        {
            idInputField.text += character;
            idInputField.caretPosition = idInputField.text.Length;
            yield return _typeDelay;
        }
        yield return _wait;
        passwordInputField.ActivateInputField();
        foreach (var character in PasswordKey.ToCharArray())
        {
            passwordInputField.text += character;
            passwordInputField.caretPosition = passwordInputField.text.Length;
            yield return _typeDelay;
        }
    }

    public void ClearInput()
    {
        idInputField.text = string.Empty;
        passwordInputField.text = string.Empty;
        idInputField.DeactivateInputField();
        passwordInputField.DeactivateInputField();
    }
}
