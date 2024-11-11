using System;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Common.Network
{
    /**
     * TODO: 세션 생성 (로그인) 및 제거 (로그아웃)
     */
    public class SessionManager: MonoBehaviour
    {
        public static SessionManager Instance = null;
        public Session currentSession;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        public async UniTask<string> SignUp(string email, string password, string confirmPassword, string playerName)
        {
            SignupData data = new SignupData();
            data.email = email;
            data.password = HashingPW(password);
            data.confirmPassword = HashingPW(confirmPassword);
            data.nickname = playerName;
            string rawData = JsonConvert.SerializeObject(data);
            Debug.Log(rawData);
            
            ResponseData response = await DataManager.Post("/api/auth/signup", rawData);
            return response.text;

        }
        public async UniTask Login(string email, string password)
        {
            if (currentSession != null)
                throw new Exception("Try to create session while already log-in");
    
            LoginData data = new LoginData();
            data.email = email;
            data.password = HashingPW(password);
            string rawData = JsonConvert.SerializeObject(data);
            Debug.Log(rawData);

            ResponseData response = await DataManager.Post("/api/auth/login", rawData);
            LoginResult result = JsonConvert.DeserializeObject<LoginResult>(response.text);
            
            // 로그인 성공 시
            currentSession = new Session();
            currentSession.token = response.token;
            currentSession.state = Session.State.Connect;
            currentSession.user = new User();
            currentSession.user.nickName = result.email;
            
            Debug.Log($"JWT: {currentSession.token}");
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }

        private string HashingPW(string pw)
        {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] encryptBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pw));
            return Convert.ToBase64String(encryptBytes);
        }
        
        class SignupData
        {
            public string email;
            public string password;
            public string confirmPassword;
            public string nickname;
        }

        class SignupResult
        {
            public string result;
        }
        
        class LoginData
        {
            public string email;
            public string password;
        }

        class LoginResult
        {
            public string success;
            public string email;
            public string response;
        }
    }
}