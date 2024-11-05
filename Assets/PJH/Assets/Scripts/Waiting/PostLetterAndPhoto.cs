using System;
using Common.Network;
using Cysharp.Threading.Tasks;
using JinsolTest.KMG.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DataManager = Common.DataManager;

public class PhotoInfo
{
    public string imageData;
    public string roomCode;
}

public class LetterInfo
{
    public string letter;
    public string roomCode;
}

public class PostLetterAndPhoto : MonoBehaviour
{
    public Button startButton;
    public RawImage photo1;
    public RawImage photo2;
    public TMP_InputField letter;
    
    private string api = "api"; //진짜 api 불러오기

    public void Start()
    {
        startButton.onClick.AddListener(PostButtonClicked);
    }

    private async void PostButtonClicked()
    {
        if (startButton.gameObject.GetComponentInChildren<TMP_Text>().text == "Start")
        {
            await PostPhoto(photo1);
            await PostPhoto(photo2);
            await PostLetter(letter.text); 
        }
}
    
    private async UniTask PostPhoto(RawImage image)
    {
        Texture2D texture = image.texture as Texture2D;

        byte[] imageBytes = texture.EncodeToJPG();
        string base64Image = System.Convert.ToBase64String(imageBytes);
        var data = new PhotoInfo
        {
            imageData = base64Image,
            roomCode = RunnerManager.Instance.Runner.SessionInfo.Name
        };
        Debug.Log(data.roomCode);

        string jsonString = JsonUtility.ToJson(data);
        try
        {
            string response = await DataManager.Instance.Post(api, jsonString, 20);
            Debug.Log("응답: " + response);
            
        }
        catch (Exception ex)
        {
            Debug.LogError("POST 요청 중 오류 발생: " + ex.Message);
        }
        
        
    }

    private async UniTask PostLetter(string letterData)
    {
        var data = new LetterInfo
        {
            letter = letterData,
            roomCode = RunnerManager.Instance.Runner.SessionInfo.Name,
        };

        string jsonString = JsonUtility.ToJson(data);
        
        try
        {
            string response = await DataManager.Instance.Post(api, jsonString, 20);
            Debug.Log("응답: " + response);
            
        }
        catch (Exception ex)
        {
            Debug.LogError("POST 요청 중 오류 발생: " + ex.Message);
        }
    }
}
