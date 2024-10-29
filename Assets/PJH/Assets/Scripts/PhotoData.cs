using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

public class PhotoData : NetworkBehaviour
{
    public int photoNumber;
    public string ownerID;

    public void SetPhotoName(int name)
    {
        photoNumber = name;
    }

    public int GetPhotoName()
    {
        return photoNumber;
    }

    public void SetOwnerID(string id)
    {
        ownerID = id;
    }

    public string GetOwnerID()
    {
        return ownerID;
    }
}
