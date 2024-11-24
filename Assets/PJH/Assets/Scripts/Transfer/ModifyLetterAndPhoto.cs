using System;
using System.Collections.Generic;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ModifyLetterAndPhoto : MonoBehaviour
{
    public static ModifyLetterAndPhoto Instance;
    private string modifyPhotoapi = "/api/photos/upload?photoId=";

    private void Awake()
    {
        Instance = this;
    }

    public async UniTask ModifyPhotoProcess(Texture2D texture, int photoId)
    {
        
        var realapi = modifyPhotoapi+ photoId;
        byte[] imageBytes = texture.EncodeToPNG();
        try
        {
            // multipart/form-data 형식으로 데이터 준비
            /*List<IMultipartFormSection> formData = new List<IMultipartFormSection>
            {
                new MultipartFormFileSection("files", imageBytes),
                new MultipartFormDataSection("roomCode", RunnerManager.Instance.Runner.SessionInfo.Name),
            };*/
            var formData2 = new WWWForm();
            formData2.AddBinaryData("files", imageBytes);
            formData2.AddField("roomCode", RunnerManager.Instance.Runner.SessionInfo.Name);
            
            // DataManager를 통해 POST 요청 보내기
            string jsonData = await DataManager.Post2(realapi, formData2);
            var response = JsonConvert.DeserializeObject<PostPhotoResponse[]>(jsonData);
            PhotoResponse.SetPostResponse(response[0]);
        }
        
        catch (Exception e)
        {
            Debug.LogError("Error sending photo info: " + e.Message);
        }
    }
}
