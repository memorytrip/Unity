using UnityEngine;

namespace Common
{
    /**
     * TODO: RESTful을 통한 파일 송/수신
     */
    public class DataManager: MonoBehaviour
    {
        public static DataManager Instance = null;
        private const string baseURL = "127.0.0.1";
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        
        private string Get(string uri)
        {
            throw new System.NotImplementedException();
        }

        private string Post(string uri, WWWForm form)
        {
            throw new System.NotImplementedException();
        }
    }
}