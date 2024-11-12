using UnityEngine;

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
