using Common.Network;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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

        private enum Panel
        {
            Login,
            Signup
        }

        private Panel panelState;

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
        }

        private void OpenSignUpPanel()
        {
            Utility.EnablePanel(signupPanel);
            openLoginPanelButton.interactable = false;
            openSignupPanelButton.interactable = false;
        }

        private void CloseLoginPanel()
        {
            Utility.DisablePanel(loginPanel);
            openLoginPanelButton.interactable = true;
            openSignupPanelButton.interactable = true;
        }

        private void CloseSignupPanel()
        {
            Utility.DisablePanel(signupPanel);
            openLoginPanelButton.interactable = true;
            openSignupPanelButton.interactable = true;
        }

        private async UniTaskVoid Login()
        {
            string id = loginIDField.text;
            string pw = loginPWField.text;
            await SessionManager.Instance.Login(id, pw);

        }

        private async UniTaskVoid Signup()
        {
            string id = signupIDField.text;
            string pw = signupPWField.text;
            string name = signupNickNameField.text;
            await SessionManager.Instance.SignUp(id, pw, pw, name);
        }
    }
}