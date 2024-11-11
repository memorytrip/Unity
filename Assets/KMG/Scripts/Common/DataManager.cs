using System;
using System.Collections.Generic;
using Common.Network;
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
        public const string baseURL = "http://memorytrip-env.eba-73mrisxy.ap-northeast-2.elasticbeanstalk.com/";

        private const string AIANALIZE = "api/ai/analyze";

        public static string token => SessionManager.Instance.currentSession.token;
        
        public static async UniTask<string> Get(string api, int timeout = 5)
        {
            string url = baseURL + api;
            UnityWebRequest request = new UnityWebRequest(url, "GET");
            //request.SetRequestHeader("authorization", token);
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

        public static async UniTask<string> Post(string api, string jsonData, int timeout = 5)
        {
            string url = baseURL + api;
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.SetRequestHeader("authorization", token);
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

        public static async UniTask<string> Post(string api, List<IMultipartFormSection> formdata, int timeout = 5)
        {
            string url =baseURL + api;
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] boundary = System.Text.Encoding.UTF8.GetBytes("----Boundary");
            //request.SetRequestHeader("authorization", token);
            request.uploadHandler = new UploadHandlerRaw(UnityWebRequest.SerializeFormSections(formdata, boundary));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = timeout;
            request.SetRequestHeader("Content-Type", "multipart/form-data; boundary=----Boundary");

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