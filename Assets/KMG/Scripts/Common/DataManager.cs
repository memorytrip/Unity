using System;
using System.Collections.Generic;
using Common.Network;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    public static class DataManager
    {
        private const string baseURL = "http://memorytrip-env.eba-73mrisxy.ap-northeast-2.elasticbeanstalk.com";

        private static string token => SessionManager.Instance.currentSession?.token;
        
        public static async UniTask<string> Get(string api, int timeout = 5)
        {
            string url = baseURL + CheckSlash(api);
            UnityWebRequest request = new UnityWebRequest(url, "GET");
            if (token != null)
                request.SetRequestHeader("Authorization", token);
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
            string url = baseURL + CheckSlash(api);
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            if (token != null)
                request.SetRequestHeader("Authorization", token);
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
            string url = baseURL + CheckSlash(api);
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] boundary = System.Text.Encoding.UTF8.GetBytes("----Boundary");
            if (token != null)
                request.SetRequestHeader("Authorization", token);
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
        
        public static async UniTask<ResponseWithToken> PostAndGetToken(string api, string jsonData, int timeout = 5)
        {
            string url = baseURL + CheckSlash(api);
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            if (token != null)
                request.SetRequestHeader("Authorization", token);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = timeout;
            request.SetRequestHeader("Content-Type", "application/json");
            
            await request.SendWebRequest();

            ResponseWithToken response = new ResponseWithToken();
            response.data = request.downloadHandler.text;
            response.token = request.GetResponseHeader("Authorization");
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                return response;
            }
            else
            {
                throw new Exception(request.error);
            }
        }

        private static string CheckSlash(string str)
        {
            if (str[0] != '/') 
                return "/" + str;
            return str;
        }
    }

    public class ResponseWithToken
    {
        public string data;
        public string token;
    }
}