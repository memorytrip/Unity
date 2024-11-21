using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GetLetterAndPhoto : MonoBehaviour
{
    public Button readyButton;
    private string endPoint = "api/photos/room/";
    private string roomCode;

    private void Awake()
    {
        roomCode = RunnerManager.Instance.Runner.SessionInfo.Name;
        endPoint += roomCode;
    }

    public void GetResponse()
    {
        GetPhotosAndLetters(endPoint).Forget();
    }

    private async UniTask GetPhotosAndLetters(string endpoint)
    {
        string response = await DataManager.Get(endpoint);
        var responseArray = JsonConvert.DeserializeObject<PhotoLetterResponse[]>(response);
        RecievedPhotoData.SetPhotoResponses(responseArray);
        Debug.Log($"사진 정보들: {responseArray}"); 
    }
    
}
