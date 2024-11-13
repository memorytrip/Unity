using System;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadLetterAndPhoto : NetworkBehaviour
{
    public RawImage[] photoImages;
    private Text[] letterTexts;  

    public override void Spawned()
    {
        LoadPhotosAndLetters().Forget();
    }
    
    private async UniTask LoadPhotosAndLetters()
    {
        try
        {
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
