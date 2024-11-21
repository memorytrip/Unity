using UnityEngine;

public class PostPhotoResponse
{
    public int photoId;
    public string photoUrl;
    public long letterId;
    public string uploaderId;
    public string finderEmail;
    public bool isFound;
    public string roomCode;
}

public class PhotoResponse : MonoBehaviour
{
    public static PostPhotoResponse postResponse { get; private set; }

    public static void SetPostResponse(PostPhotoResponse response)
    {
        postResponse = response;
    }

    public static void PostResponseClear()
    {
        postResponse = null;
    }
}
