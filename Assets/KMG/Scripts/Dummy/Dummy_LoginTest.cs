using System;
using Common.Network;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Exception = System.Exception;

public class Dummy_LoginTest : MonoBehaviour
{
    [Header("Login Panel")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField loginIDField;
    [SerializeField] private TMP_InputField loginPWField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button gotoSignUpButton;
    
    [Header("Signup Panel")]
    [SerializeField] private GameObject signupPanel;
    [SerializeField] private TMP_InputField signupIDField;
    [SerializeField] private TMP_InputField signupPWField;
    [SerializeField] private TMP_InputField signupNickNameField;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button gotoLoginButton;

    private enum Panel
    {
        Login,
        Signup
    }

    private Panel panelState;

    private void Awake()
    {
        gotoSignUpButton.onClick.AddListener(SwitchPanel);
        gotoLoginButton.onClick.AddListener(SwitchPanel);
        loginButton.onClick.AddListener(()=>Login().Forget());
        signUpButton.onClick.AddListener(()=>Signup().Forget());
    }

    private void SwitchPanel()
    {
        switch (panelState)
        {
            case Panel.Login:
                loginPanel.SetActive(false);
                signupPanel.SetActive(true);
                panelState = Panel.Signup;
                break;
            case Panel.Signup:
                loginPanel.SetActive(true);
                signupPanel.SetActive(false);
                panelState = Panel.Login;
                break;
        }
    }

    private async UniTaskVoid Login()
    {
        string id = loginIDField.text;
        string pw = loginPWField.text;
        try
        {
            await SessionManager.Instance.Login(id, pw);
        }
        catch (Exception e)
        {
            Debug.LogAssertion(e.Message);
        }
        
    }

    private async UniTaskVoid Signup()
    {
        string id = signupIDField.text;
        string pw = signupPWField.text;
        string name = signupNickNameField.text;
        try
        {
            await SessionManager.Instance.SignUp(id, pw, name);
        }
        catch (Exception e)
        {
            Debug.LogAssertion(e.Message);
        }
    }
}
