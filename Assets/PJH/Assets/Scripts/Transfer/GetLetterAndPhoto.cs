using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetLetterAndPhoto : MonoBehaviour
{
    public RawImage[] photoImages; // Inspector에서 8개의 RawImage 할당
    private Text[] letterTexts;     // Inspector에서 8개의 Text 할당
    private string endPoint = "api/photos/user/";
    private string userNickName;

    private void Awake()
    {
        userNickName = "1";
        endPoint += userNickName;
    }

    public async void ButtonClicked()
    {
        await LoadPhotosAndLetters(endPoint);
        if (letterTexts == null)
        {
            Debug.Log("1");
        }
        /*for (int i = 0; i < letterTexts.Length; i++)
        {
            Debug.Log(letterTexts[i]);
        }*/
    }

    public async UniTask LoadPhotosAndLetters(string endpoint)
    {
        try
        {
            // GET 요청으로 데이터 받아오기
            /*string response = await DataManager.Get(endpoint);
            
            PhotoLetterResponse[] data = JsonConvert.DeserializeObject<PhotoLetterResponse[]>(response);*/

            var data = RecievedPhotoData.PhotoResponses;
            
            // 각 이미지와 편지 처리
            for (int i = 0; i < data.Length && i < photoImages.Length; i++)
            {
                await LoadSingleImage(data[i], i);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading photos and letters: {e.Message}");
        }
    }

    private async UniTask LoadSingleImage(PhotoLetterResponse item, int index)
    {
        try
        {
            // 이미지 로드
            string url = item.photoUrl;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

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
                    letterTexts[index].text = item.letterId;
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
