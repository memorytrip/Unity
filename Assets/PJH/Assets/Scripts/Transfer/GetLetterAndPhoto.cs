using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PhotoLetterResponse
{
    public PhotoLetterData[] items;
}

[System.Serializable]
public class PhotoLetterData
{
    public string imageUrl;
    public string letter;
    // 필요한 경우 추가 필드들
    public string userID;
    public string roomCode;
}


public class GetLetterAndPhoto : MonoBehaviour
{
    /*//json으로 Get
    private async UniTask GetPhoto(string roomCode)
    {
        string response = await DataManager.Get("api/asd"); 
        var responseData = JsonConvert.DeserializeObject<GetResponse>(response);

        for (int i = 0; i < responseData.userCount; i++)
        {
            byte[] imageBytes = Convert.FromBase64String(responseData.letter[i].ToString());
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            //AspectFix(출력하는 이미지UI, texture);
        }
    }*/
    
    public RawImage[] photoImages; // Inspector에서 8개의 RawImage 할당
    private Text[] letterTexts;     // Inspector에서 8개의 Text 할당
    private const string endPoint = "";

    public async void ButtonClicked()
    {
        await LoadPhotosAndLetters(endPoint);
        for (int i = 0; i < letterTexts.Length; i++)
        {
            Debug.Log(letterTexts[i]);
        }
    }
    
    public async UniTask LoadPhotosAndLetters(string endpoint)
    {
        try
        {
            // GET 요청으로 데이터 받아오기
            string response = await DataManager.Get(endpoint);
            
            // JSON 응답을 객체로 변환
            PhotoLetterResponse data = JsonUtility.FromJson<PhotoLetterResponse>(response);
            
            // 각 이미지와 편지 처리
            for (int i = 0; i < data.items.Length && i < photoImages.Length; i++)
            {
                await LoadSingleImage(data.items[i], i);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading photos and letters: {e.Message}");
        }
    }

    private async UniTask LoadSingleImage(PhotoLetterData item, int index)
    {
        try
        {
            // 이미지 로드
            string url = DataManager.baseURL + item.imageUrl;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            request.SetRequestHeader("authorization", DataManager.token);
            
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 이미지 표시
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                photoImages[index].texture = texture;
                
                // 이미지 비율 맞추기
                await AdjustImageAspectRatio(photoImages[index], texture);
                
                // 편지 텍스트 표시
                if (letterTexts[index] != null)
                {
                    letterTexts[index].text = item.letter;
                }
            }
            else
            {
                Debug.LogError($"Failed to load image {index}: {request.error}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading image {index}: {e.Message}");
        }
    }

    private async UniTask AdjustImageAspectRatio(RawImage image, Texture2D texture)
    {
        await UniTask.WaitForEndOfFrame();

        RectTransform rectTransform = image.GetComponent<RectTransform>();
        float textureRatio = (float)texture.width / texture.height;
        float targetRatio = rectTransform.rect.width / rectTransform.rect.height;

        if (textureRatio > targetRatio)
        {
            // 가로에 맞추기
            float height = rectTransform.rect.width / textureRatio;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        }
        else
        {
            // 세로에 맞추기
            float width = rectTransform.rect.height * textureRatio;
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }
    }

    // 메모리 관리
    private void OnDestroy()
    {
        foreach (RawImage image in photoImages)
        {
            if (image != null && image.texture != null)
            {
                Destroy(image.texture);
            }
        }
    }

    
    
}
