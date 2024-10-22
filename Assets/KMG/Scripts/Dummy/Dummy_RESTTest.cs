using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Dummy_RESTTest : MonoBehaviour
{
    [SerializeField] private Button getButton;
    [SerializeField] private Button postButton;

    [Serializable]
    class TestData
    {
        public string comfyui;
        public string ai_video;
        public string ai_music;
    }
    
    void Start()
    { 
        getButton.onClick.AddListener(()=>Get().Forget());
        postButton.onClick.AddListener(()=>Post().Forget());
    }

    private async UniTaskVoid Post()
    {
        TestData testData = new TestData();
        testData.comfyui = "adsf";
        testData.ai_video = "asdffd";
        testData.ai_music = "fdsa";
        // JSON 문자열로 변환
        string jsonData = JsonConvert.SerializeObject(testData);

        // 요청 생성
        UnityWebRequest request = new UnityWebRequest("http://125.132.216.190:12222/api/ai/analyze", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.timeout = 60;
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        // 요청 보내기
        await request.SendWebRequest();

        // 응답 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Successfully sent data: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
    
    private async UniTaskVoid Get()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://125.132.216.190:12221/unity/get");
        await www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.responseCode + www.downloadHandler.text);
        }
    }
}
