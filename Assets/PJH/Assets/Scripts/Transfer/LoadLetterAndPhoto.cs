using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadLetterAndPhoto : NetworkBehaviour
{
    public List<string> selectedPhotoUrl;
    public Button[] photoButtons;
    public GameObject[] lockImage;
    private RawImage[] photoImages;
    private Text[] letterTexts;
    
    public override void Spawned()
    {
        photoImages = new RawImage[photoButtons.Length];
        for (int i = 0; i < photoButtons.Length; i++)
        {
            lockImage[i].SetActive(true);
            RawImage rawImage = photoButtons[i].GetComponentInChildren<RawImage>();
            photoImages[i] = rawImage; 
        }
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
                Debug.Log("데이터:" + data[i]);
                if (data[i] != null)
                {
                    photoButtons[i].GetComponent<CanvasGroup>().interactable = true;
                    photoButtons[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
                    lockImage[i].SetActive(false);
                    await LoadSingleImage(data[i], i);
                    int index = i;
                    //photoButtons[index].onClick.RemoveAllListeners();
                    photoButtons[index].onClick.AddListener(() => SetPhotoUrlToButton(data[index].photoUrl));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading photos and letters: {e.Message}");
        }
    }

    private void SetPhotoUrlToButton(string photoUrl)
    {
        if (!selectedPhotoUrl.Contains(photoUrl))
        {
            selectedPhotoUrl.Add(photoUrl);
            Debug.Log($"URL 추가됨: {photoUrl}");
        }
        else
        {
            selectedPhotoUrl.Remove(photoUrl);
            Debug.Log($"URL 제거됨: {photoUrl}");
        }
    }
    
    private async UniTask LoadSingleImage(PhotoLetterResponse item, int index)
    {
        try
        {
            // 이미지 로드
            string url = item.photoUrl;

            if (url == null)
            {
                return;
            }
            
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 이미지 표시
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                photoImages[index].texture = texture;

                // 이미지 비율 맞추기
                //await AdjustImageAspectRatio(photoImages[index], texture);
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

    /*private void OnDestroy()
    {
        foreach (RawImage image in photoImages)
        {
            if (image != null && image.texture != null)
            {
                Destroy(image.texture);
            }
        }
    }*/
}
