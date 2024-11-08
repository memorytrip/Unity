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
    [SerializeField] private Button triggerButton;
    [SerializeField] private Button triggerClearButton;

    private void Awake()
    {
        triggerButton.onClick.AddListener(StartInput);
        triggerClearButton.onClick.AddListener(ClearInput);
    }
    
    public void StartInput()
    {
        StartCoroutine(AutomateInput());
    }
    
    private IEnumerator AutomateInput()
    {
        yield return _wait;
        idInputField.text = "Traver";
        yield return _wait;
        passwordInputField.text = "Password!1234"; // 기절할 비밀번호 조합
    }

    public void ClearInput()
    {
        idInputField.text = string.Empty;
        passwordInputField.text = string.Empty;
    }
}
