using UnityEngine;

namespace Common.Network
{
    /**
     * TODO: 세션 생성 (로그인) 및 제거 (로그아웃)
     */
    public class SessionManager: MonoBehaviour
    {
        public static SessionManager Instance = null;
        private string baseURL = "127.0.0.1";
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        public void Login()
        {
            throw new System.NotImplementedException();
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }
    }
}