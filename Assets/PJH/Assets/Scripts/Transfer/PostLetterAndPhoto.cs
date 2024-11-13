using System;
using System.Collections.Generic;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DataManager = Common.DataManager;

[System.Serializable]
public class PhotoLetterResponse
{
    public long photoId;
    public string photoUrl;
    public long likeCount;
    public string letterId;
    public long uploaderId;
    public bool isFound;
}

public class PhotoInfo
{
    public string imageData;
    public string roomCode;
    public string letter;
    public string userCount;
    public string userID;
}

public class PostLetterAndPhoto : MonoBehaviour
{
    public static PostLetterAndPhoto Instance;

    public PlayReadyRoom playReadyRoom;
    public GetLetterAndPhoto getLetterAndPhoto;
    public Button startButton;
    public RawImage photo1;
    public RawImage photo2;
    //public TMP_InputField letter;
    private string endPoint = "api/photos/room";

    private string api = "/api/photos/upload"; // + userID; //진짜 api 불러오기
    private void Awake()
    {
        Instance = this;
        RecievedPhotoData.Clear();
    }
    
    public void Start()
    {
        startButton.onClick.AddListener(PostButtonClicked);
    }
    
    private async void PostButtonClicked()
    {
        if (startButton.gameObject.GetComponentInChildren<TMP_Text>().text == "시작하기")
        {
            await PostPhoto(photo1, "" /*letter.text*/);
            await PostPhoto(photo2, "");
            //await PostLetter(letter.text); 
        }
        else
        {
            Debug.Log("시작X");
        }
    }
    
    //formData로 Post
    private async UniTask PostPhoto(RawImage photo, string letterInfo)
    {
        Texture2D texture = photo.texture as Texture2D;

        byte[] imageBytes = texture.EncodeToPNG();
        try
        {
            // multipart/form-data 형식으로 데이터 준비
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>
            {
                new MultipartFormDataSection("photoUrl", imageBytes),
                new MultipartFormDataSection("roomCode", RunnerManager.Instance.Runner.SessionInfo.Name),
                new MultipartFormDataSection("UploaderId", SessionManager.Instance.currentUser.email),
                //new MultipartFormDataSection("Letter", letterInfo)
            };
            // DataManager를 통해 POST 요청 보내기
            string response = await DataManager.Post(api, formData);
            
            // 성공적으로 응답을 받았을 때의 처리
            Debug.Log("Post성공");
            /*getLetterAndPhoto.GetResponse().Forget();
            Debug.Log("겟도 성공");
            playReadyRoom.RpcGameStart();
            Debug.Log("게임 시작했는데...?");*/
            
            /*var responseArray = JsonConvert.DeserializeObject<PhotoLetterResponse[]>(response);
            RecievedPhotoData.SetPhotoResponses(responseArray);*/
        }
        
        catch (Exception e)
        {
            // 에러 처리
            Debug.LogError("Error sending photo info: " + e.Message);
        }
    }
    
}