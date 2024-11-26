using Common;
using System.Collections;
using Common.Network;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Exception = System.Exception;

namespace GUI
{
    public class LoginUI : MonoBehaviour
    {
        [Header("Button")] 
        [SerializeField] private Button openLoginPanelButton;
        [SerializeField] private Button openSignupPanelButton;
        
        [Header("Login Panel")] 
        [SerializeField] private CanvasGroup loginPanel;

        [SerializeField] private TMP_InputField loginIDField;
        [SerializeField] private TMP_InputField loginPWField;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button closeLoginPanelButton;

        [Header("Signup Panel")] 
        [SerializeField] private CanvasGroup signupPanel;

        [SerializeField] private TMP_InputField signupIDField;
        [SerializeField] private TMP_InputField signupPWField;
        [SerializeField] private TMP_InputField signupConfirmPWField;
        [SerializeField] private TMP_InputField signupNickNameField;
        [SerializeField] private Button signUpButton;
        [SerializeField] private Button closeSignupPanelButton;

        [Header("Popup Panel")] 
        [SerializeField] private CanvasGroup popupPanel;
        [SerializeField] private TMP_Text popupText;
        [SerializeField] private Button popupButton;

        private readonly WaitForSeconds _wait = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds _typeDelay = new WaitForSeconds(0.05f);
        private const string IdKey = "Traver";
        private const string PasswordKey = "Pass!1234";
        private const string NicknameKey = "트래버";

        private void Awake()
        {
            openLoginPanelButton.onClick.AddListener(OpenLoginPanel);
            openSignupPanelButton.onClick.AddListener(OpenSignUpPanel);
            closeLoginPanelButton.onClick.AddListener(CloseLoginPanel);
            closeSignupPanelButton.onClick.AddListener(CloseSignupPanel);
            loginButton.onClick.AddListener(() => Login().Forget());
            signUpButton.onClick.AddListener(() => Signup().Forget());
            popupButton.onClick.AddListener(ClosePopupPanel);
        }

        private void OpenLoginPanel()
        {
            Utility.EnablePanel(loginPanel);
            openLoginPanelButton.interactable = false;
            openSignupPanelButton.interactable = false;
            StartCoroutine(AutomateLoginInput());
        }

        private void OpenSignUpPanel()
        {
            Utility.EnablePanel(signupPanel);
            openLoginPanelButton.interactable = false;
            openSignupPanelButton.interactable = false;
            StartCoroutine(AutomateSignupInput());
        }

        private void OpenPopupPanel(string message = "")
        {
            Utility.EnablePanel(popupPanel);
            popupText.text = message;
            openLoginPanelButton.interactable = false;
            openSignupPanelButton.interactable = false;
        }

        private void CloseLoginPanel()
        {
            ClearForm(loginIDField);
            ClearForm(loginPWField);
            Utility.DisablePanel(loginPanel);
            openLoginPanelButton.interactable = true;
            openSignupPanelButton.interactable = true;
        }

        private void CloseSignupPanel()
        {
            ClearForm(signupIDField);
            ClearForm(signupNickNameField);
            ClearForm(signupPWField);
            ClearForm(signupConfirmPWField);
            Utility.DisablePanel(signupPanel);
            openLoginPanelButton.interactable = true;
            openSignupPanelButton.interactable = true;
        }

        private void ClosePopupPanel()
        {
            Utility.DisablePanel(popupPanel);
            openLoginPanelButton.interactable = true;
            openSignupPanelButton.interactable = true;
        }

        private async UniTaskVoid Login()
        {
            string id = loginIDField.text;
            string pw = loginPWField.text;

            loginButton.interactable = false;
            closeLoginPanelButton.interactable = false;
            
            try
            {
                await SessionManager.Instance.Login(id, pw);
            }
            catch (UnityWebRequestException e)
            {
                
                Debug.LogAssertion(e.Message);
                if (e.ResponseHeaders != null && e.ResponseHeaders["Content-Type"] == "application/json")
                {
                    ErrorResult error = JsonConvert.DeserializeObject<ErrorResult>(e.Text);
                    OpenPopupPanel(error.response);    
                }
                else
                {
                    OpenPopupPanel(e.Error);
                }
                loginButton.interactable = true;
                closeLoginPanelButton.interactable = true;
                return;
            }

            try
            {
                await SceneManager.Instance.MoveRoom(SceneName.Square); 
            }
            catch (Exception e)
            { 
                Debug.LogAssertion(e);
                OpenPopupPanel(e.Message);
                loginButton.interactable = true;
                closeLoginPanelButton.interactable = true;
            }
        }

        private async UniTaskVoid Signup()
        {
            string id = signupIDField.text;
            string pw = signupPWField.text;
            string confirmPw = signupConfirmPWField.text;
            string name = signupNickNameField.text;

            signUpButton.interactable = false;
            closeSignupPanelButton.interactable = false;
            
            try
            {
                await SessionManager.Instance.SignUp(id, pw, confirmPw, name);
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogAssertion(e.Message);
                if (e.ResponseHeaders != null && e.ResponseHeaders["Content-Type"] == "application/json")
                {
                    ErrorResult error = JsonConvert.DeserializeObject<ErrorResult>(e.Text);
                    OpenPopupPanel(error.response);
                }
                else
                {
                    OpenPopupPanel(e.Error);   
                }
                signUpButton.interactable = true;
                closeSignupPanelButton.interactable = true;
                return;
            }
            
            signUpButton.interactable = true;
            closeSignupPanelButton.interactable = true;
            CloseSignupPanel();
            OpenPopupPanel("회원가입이 완료되었습니다");
        }
        
        private IEnumerator AutomateLoginInput()
        {
            yield return _wait;
            loginIDField.ActivateInputField();
            foreach (var character in IdKey.ToCharArray())
            {
                loginIDField.text += character;
                loginIDField.caretPosition = loginIDField.text.Length;
                yield return _typeDelay;
            }
            yield return _wait;
            loginPWField.ActivateInputField();
            foreach (var character in PasswordKey.ToCharArray())
            {
                loginPWField.text += character;
                loginPWField.caretPosition = loginPWField.text.Length;
                yield return _typeDelay;
            }
        }

        private IEnumerator AutomateSignupInput()
        {
            yield return _wait;
            signupIDField.ActivateInputField();
            foreach (var character in IdKey.ToCharArray())
            {
                signupIDField.text += character;
                signupIDField.caretPosition = signupIDField.text.Length;
                yield return _typeDelay;
            }
            yield return _wait;
            signupNickNameField.ActivateInputField();
            foreach (var character in NicknameKey.ToCharArray())
            {
                signupNickNameField.text += character;
                signupNickNameField.caretPosition = signupNickNameField.text.Length;
                yield return _typeDelay;
            }
            yield return _wait;
            signupPWField.ActivateInputField();
            foreach (var character in PasswordKey.ToCharArray())
            {
                signupPWField.text += character;
                signupPWField.caretPosition = signupPWField.text.Length;
                yield return _typeDelay;
            }
            yield return _wait;
            signupConfirmPWField.ActivateInputField();
            foreach (var character in PasswordKey.ToCharArray())
            {
                signupConfirmPWField.text += character;
                signupConfirmPWField.caretPosition = signupConfirmPWField.text.Length;
                yield return _typeDelay;
            }
        }

        private void ClearForm(TMP_InputField inputField)
        {
            StopAllCoroutines();
            inputField.text = string.Empty;
            inputField.DeactivateInputField();
        }

        class ErrorResult
        {
            public bool success;
            public string response;
        }
    }
}