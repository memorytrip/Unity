using System;
using Common;
using Cysharp.Threading.Tasks;
using Fusion;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LetterInPhoto : NetworkBehaviour
{
    public Button[] letterPhotoButtons;
    public RawImage[] popUpPhotoImages;
    public RawImage[] letterPhotoImages;
    public TMP_Text[] letters;
    public GameObject[] lockImage;
    private string endPoint = "api/letters";
    
    public override void Spawned()
    {
        LoadLetterPhoto().Forget();
    }

    private async UniTask LoadLetterPhoto()
    {
        try
        {
            var data = RecievedPhotoData.PhotoResponses;
            Debug.Log("사진에 대해 받은 사진 GET한거:" + data);

            // 각 이미지와 편지 처리
            for (int i = 0; i < data.Length; i++)
            {
                int j = 0;
                if (data[i] != null && data[i].letterId != null)
                {
                    var realapi = endPoint +"/"+ data[i].letterId + "/details";
                    Debug.Log(endPoint);
                    await GetLetterdata(realapi);

                    var item = LetterInfo.letterResponse;

                    if (item.content != null)
                    {
                        lockImage[j].SetActive(false);
                        letterPhotoButtons[j].GetComponent<CanvasGroup>().interactable = true;
                        letterPhotoButtons[j].GetComponent<CanvasGroup>().blocksRaycasts = true;
                        await LoadSingleImage(data[i], j);
                        letters[j].text = item.content;
                    }
                    Debug.Log($"j ={j}, i={j}");
                    j++;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading photos and letters: {e.Message}");
        }
    }

    private async UniTask GetLetterdata(string endpoint)
    {
        string response = await DataManager.Get(endpoint);
        var letter = JsonConvert.DeserializeObject<RecievedLetterData>(response);
        LetterInfo.SetLetterResponse(letter);
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
                letterPhotoImages[index].texture = texture;
                popUpPhotoImages[index].texture = texture;

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
    
    

}
