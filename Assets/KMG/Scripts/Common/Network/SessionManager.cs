using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

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

        public async UniTask<string> SignUp(string email, string password, string playerName)
        {
            SignupData data = new SignupData();
            data.email = email;
            data.password = password;
            data.playerName = playerName;
            string rawData = JsonConvert.SerializeObject(data);
            
            string response = await DataManager.Post("/signup", rawData);
            return response;

        }
        public async UniTask Login(string email, string password)
        {
            if (currentSession != null)
                throw new Exception("Try to create session while current session already occupied");
    
            LoginData data = new LoginData();
            data.email = email;
            data.password = password;
            string rawData = JsonConvert.SerializeObject(data);

            string response = await DataManager.Post("/login", rawData);
            LoginResult result = JsonConvert.DeserializeObject<LoginResult>(response);
            
            // 로그인 성공 시
            currentSession = new Session();
            currentSession.token = result.token;
            currentSession.state = Session.State.Connect;
            currentSession.user = new User();
            currentSession.user.email = email;
            currentSession.user.nickName = result.nickname;
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }

        private void CreateSession()
        {
            
        }
        
        class SignupData
        {
            public string email;
            public string password;
            public string playerName;
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
            public string result;
            public string nickname;
            public string token;
        }
    }
}