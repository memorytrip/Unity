using Common;
using System.Collections;
using Common.Network;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        private async UniTaskVoid Login()
        {
            string id = loginIDField.text;
            string pw = loginPWField.text;
            await SessionManager.Instance.Login(id, pw);
            await SceneManager.Instance.MoveRoom(SceneManager.SquareScene);

        }

        private async UniTaskVoid Signup()
        {
            string id = signupIDField.text;
            string pw = signupPWField.text;
            string confirmPw = signupConfirmPWField.text;
            string name = signupNickNameField.text;
            await SessionManager.Instance.SignUp(id, pw, confirmPw, name);
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
            inputField.text = string.Empty;
            inputField.DeactivateInputField();
        }
    }
}