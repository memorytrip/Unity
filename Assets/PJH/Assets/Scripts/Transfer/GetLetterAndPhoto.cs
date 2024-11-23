using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GetLetterAndPhoto : MonoBehaviour
{
    public Button readyButton;
    private string endPoint = "api/photos/room/";
    private string roomCode;

    private void Awake()
    {
        roomCode = RunnerManager.Instance.Runner.SessionInfo.Name;
    }

    public void GetResponse()
    {
        var realapi = endPoint + roomCode;
        GetPhotosAndLetters(realapi).Forget();
    }

    private async UniTask GetPhotosAndLetters(string endpoint)
    {
        string response = await DataManager.Get(endpoint);
        var responseArray = JsonConvert.DeserializeObject<PhotoLetterResponse[]>(response);
        RecievedPhotoData.SetPhotoResponses(responseArray);
        for (int i = 0; i < responseArray.Length; i++)
        {
            Debug.Log($"사진 정보들: {responseArray[i].letterId} , {responseArray[i].photoId}"); 
        }
    }
    
}
