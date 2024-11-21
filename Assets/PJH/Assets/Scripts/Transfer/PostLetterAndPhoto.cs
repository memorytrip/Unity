using System;
using System.Collections.Generic;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DataManager = Common.DataManager;

public class PostLetterAndPhoto : MonoBehaviour
{
    public static PostLetterAndPhoto Instance;

    public TMP_InputField letter;
    public PlayReadyRoom playReadyRoom;
    public GetLetterAndPhoto getLetterAndPhoto;
    public Button startButton;
    public RawImage photo1;
    public RawImage photo2;
    private string letterapi = "api/letters";
    private string photoapi = "/api/photos/upload";
    
    
    private void Awake()
    {
        Instance = this;
        RecievedPhotoData.Clear();
        PhotoResponse.PostResponseClear();
    }

    private void Start()
    {
        startButton.onClick.AddListener(CheckAndExecute);
    }

    private async void CheckAndExecute()
    {
        if (startButton.GetComponentInChildren<TMP_Text>().text =="준비완료")
        {
            await PostLetterProcess(letter.text);
        }
        else
        {
            Debug.Log("준비완료가 아님");
        }
    }

    public void PostPhoto(RawImage photo)
    {   
        PostPhotoProcess(photo).Forget();
    }
    
    /*public void Start()
    {
        startButton.onClick.AddListener(PostButtonClicked);
    }
    
    private async void PostButtonClicked()
    {
        if (startButton.gameObject.GetComponentInChildren<TMP_Text>().text == "시작하기")
        {
            await PostPhoto(photo1);
            await PostPhoto(photo2);
            //await PostLetter(letter.text); 
        }
        else
        {
            Debug.Log("시작X");
        }
    }*/
    
    //formData로 Post
    private async UniTask PostPhotoProcess(RawImage photo)
    {
        Texture2D texture = photo.texture as Texture2D;

        byte[] imageBytes = texture.EncodeToPNG();
        try
        {
            // multipart/form-data 형식으로 데이터 준비
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>
            {
                new MultipartFormDataSection("files", imageBytes),
                new MultipartFormDataSection("roomCode", RunnerManager.Instance.Runner.SessionInfo.Name),
            };
            
            // DataManager를 통해 POST 요청 보내기
            string jsonData = await DataManager.Post(photoapi, formData);
            var response = JsonConvert.DeserializeObject<PostPhotoResponse>(jsonData);
            PhotoResponse.SetPostResponse(response);
            Debug.Log("사진 Post성공");

        }
        
        catch (Exception e)
        {
            Debug.LogError("Error sending photo info: " + e.Message);
        }
    }

    private async UniTask PostLetterProcess(string letterInfo)
    {
        letterapi += "?photoId="+PhotoResponse.postResponse.photoId;
        try
        {
            // multipart/form-data 형식으로 데이터 준비
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>
            {
                new MultipartFormDataSection("content", letterInfo),
            };
            
            // DataManager를 통해 POST 요청 보내기
            await DataManager.Post(letterapi, formData);
            Debug.Log("편지 Post성공");
        }
        
        catch (Exception e)
        {
            Debug.LogError("Error sending letter info: " + e.Message);
        }
    }
}