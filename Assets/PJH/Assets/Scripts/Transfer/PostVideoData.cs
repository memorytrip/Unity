using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Exception = System.Exception;

public class VideoInfo
{
    public string roomCode;
    public string[] photoUrls;
    public string text;
}

public class PostVideoData : MonoBehaviour
{
    //public RawImage[] selectedImages;
    public TMP_InputField emotionText;
    public LoadLetterAndPhoto loadLP;
    private string endPoint = "/api/videos/";

    public void PostVideo()
    {
        PostVideoProcess().Forget();
    }

    public async UniTask PostVideoProcess()
    {
        string realapi = endPoint + "request-video";
        try
        {
            for (int i = 0; i < loadLP.selectedPhotoUrl.Count; i++)
            {
                Debug.Log(loadLP.selectedPhotoUrl[i]);
            } //여기나오는지 확인해야함

            var data = new VideoInfo
            {
                roomCode = RunnerManager.Instance.Runner.SessionInfo.Name,
                photoUrls = loadLP.selectedPhotoUrl.ToArray(),
                text = emotionText.text
            };
            string jsonData = JsonConvert.SerializeObject(data);

            await DataManager.Post(realapi, jsonData);
            Debug.Log("비디오 정보 Post 성공");

        }

        catch (Exception e)
        {
            Debug.LogError("Error sending photo info: " + e.Message);
        }
    }


}
