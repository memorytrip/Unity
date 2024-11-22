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
    public RawImage[] letterPhotoImages;
    public TMP_Text[] letters;
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

            // 각 이미지와 편지 처리
            for (int i = 0; i < data.Length && i < letterPhotoImages.Length; i++)
            {
                if (data[i] != null)
                {
                    endPoint += data[i].letterId + "/details";
                    await GetLetterdata(endPoint);

                    var item = LetterInfo.letterResponse;
                    //여기에 받은 letterInfo에 content를 열어보고 null이 아닌 사진과 content를 가져와야하는데 왜 안떠 ㅡㅡ
                    if (item.content != null)
                    {
                        await LoadSingleImage(data[i], i);
                        letters[i].text = item.content;
                    }
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
