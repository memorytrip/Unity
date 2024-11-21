using UnityEngine;

[System.Serializable]
public class PhotoLetterResponse
{
    public long photoId;
    public string photoUrl;
    public long? letterId;
    public string uploaderEmail;
    public string finderEmail;
    public bool isFound;
    public string roomCode;
}

public class RecievedPhotoData : MonoBehaviour
{
    public static PhotoLetterResponse[] PhotoResponses { get; private set; }
    
    public static void SetPhotoResponses(PhotoLetterResponse[] responses)
    {
        PhotoResponses = responses;
    }
    
    public static void Clear()
    {
        PhotoResponses = null;
    }
}
