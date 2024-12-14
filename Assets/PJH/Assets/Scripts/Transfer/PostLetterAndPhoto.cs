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

class LetterData
{
    public string content;
}

class LetterResponseData
{
    public string content;
    public int? letterId;
}

public class PostLetterAndPhoto : MonoBehaviour
{
    public static PostLetterAndPhoto Instance;

    public TMP_InputField letter;
    public GameObject photo1;
    private string letterapi = "api/letters";
    private string photoapi = "/api/photos/upload";
    private UploadPhoto up;
    private int? letterId;
    
    private void Awake()
    {
        Instance = this;
        RecievedPhotoData.Clear();
        PhotoResponse.PostResponseClear();
        up = photo1.GetComponent<UploadPhoto>();
    }

    public void CheckAndExecute()
    {
        if (letterId == null)
        {
            PostLetterProcess(letter.text).Forget();
        }
        else
        {
            PutLetterProcess(letter.text).Forget();
        }
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
    public async UniTask PostPhotoProcess(Texture2D texture)
    {
        Texture2D resizedTexture = ResizeTexture(texture, 720, 480);
        byte[] imageBytes = resizedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/resizedImage.png", imageBytes);
        Debug.Log($"Image saved at: {Application.persistentDataPath}/resizedImage.png");
        Debug.Log($"실제 이미지 해상도: {texture.width} x {texture.height}");
        Debug.Log($"리사이즈 이미지 해상도: {resizedTexture.width} x {resizedTexture.height}");
        Debug.Log($"이미지 바이트 크기: {imageBytes.Length} bytes");
        
        Debug.Log("이미지 배열:" + imageBytes);
        try
        {
            // multipart/form-data 형식으로 데이터 준비
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>
            {
                new MultipartFormFileSection("files", imageBytes),
                new MultipartFormDataSection("roomCode", RunnerManager.Instance.Runner.SessionInfo.Name),
            };
            var formData2 = new WWWForm();
            formData2.AddBinaryData("files", imageBytes);
            formData2.AddField("roomCode", RunnerManager.Instance.Runner.SessionInfo.Name);
            
            // DataManager를 통해 POST 요청 보내기
            string jsonData = await DataManager.Post2(photoapi, formData2);
            var response = JsonConvert.DeserializeObject<PostPhotoResponse[]>(jsonData);
            PhotoResponse.SetPostResponse(response[0]);
            Debug.Log("사진 Post성공");

        }   
        
        catch (Exception e)
        {
            Debug.LogError("Error sending photo info: " + e.Message);
        }
    }

    private async UniTask PostLetterProcess(string letterInfo)
    {
        var realapi = letterapi +"?photoId="+ up.photoId;
        try
        {
            var data = new LetterData
            {
                content = letterInfo
            };
            // multipart/form-data 형식으로 데이터 준비
            var jsonData = JsonConvert.SerializeObject(data);
            
            // DataManager를 통해 POST 요청 보내기
            string jsonResponse = await DataManager.Post(realapi, jsonData, 300);
            Debug.Log("편지 Post성공");

            var response= JsonConvert.DeserializeObject<LetterResponseData>(jsonResponse);
            if (response.letterId != null)
            {
                letterId = response.letterId;
            }
            
        }
        
        catch (Exception e)
        {
            Debug.LogError("Error sending letter info: " + e.Message);
        }
    }
    
    private async UniTask PutLetterProcess(string letterInfo)
    {
        var realapi = letterapi + "/" + letterId;
        try
        {
            var data = new LetterData
            {
                content = letterInfo
            };
            // multipart/form-data 형식으로 데이터 준비
            var jsonData = JsonConvert.SerializeObject(data);
            
            // DataManager를 통해 POST 요청 보내기
            await DataManager.Put(realapi, jsonData);
            Debug.Log("편지 Post성공");
        }
        
        catch (Exception e)
        {
            Debug.LogError("Error sending letter info: " + e.Message);
        }
    }
    
    public Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
        RenderTexture.active = rt;

        Graphics.Blit(source, rt);

        Texture2D result = new Texture2D(targetWidth, targetHeight);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
    
}