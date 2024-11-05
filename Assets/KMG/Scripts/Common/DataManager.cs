using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    /**
     * TODO: RESTful을 통한 json 송/수신
     */
    public class DataManager: MonoBehaviour
    {
        public static DataManager Instance = null;
        private const string baseURL = "http://125.132.216.190:12222/";

        private const string AIANALIZE = "api/ai/analyze";
        
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        
        private async UniTask<string> Get(string api, int timeout = 5)
        {
            string url = baseURL + api;
            UnityWebRequest request = new UnityWebRequest(url, "GET");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = timeout;

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                throw new Exception(request.error);
            }
        }

        public async UniTask<string> Post(string api, string jsonData, int timeout = 5)
        {
            string url = baseURL + api;
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = timeout;
            request.SetRequestHeader("Content-Type", "application/json");
            
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                throw new Exception(request.error);
            }
        }
    }
}