using System.Collections;
using System.Collections.Generic;
using Common.Network;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Fusion;
using TMPro;

public class PhotoManager : NetworkBehaviour, IListener
{
    private float yValue = 1f;
    private Dictionary<int, Vector3> photoPositions;
    public NetworkPrefabRef photoPrefab;
    private GameObject hitObject;
    private int findedPhoto = 0;
    private NetworkObject hitNetworkObject;

    void Start()
    {
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
        photoPositions = new Dictionary<int, Vector3>();
        HidePhoto(photoPositions, 8).Forget();
    }
    
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10, 11), yValue, Random.Range(-10, 11));
    }

    public async UniTaskVoid HidePhoto(Dictionary<int, Vector3> photoDict, int numberOfPhotos)
    {
        for (var i = 1; i <= numberOfPhotos; i++)
        {
            int photoName = i;
            photoDict[photoName] = GetRandomPosition(); 
            var photoObject = await RunnerManager.Instance.Runner.SpawnAsync(photoPrefab, photoDict[photoName], Quaternion.identity);
            // NetworkObject photoObject = RunnerManager.Instance.Runner.Spawn(photoPrefab, photoDict[photoName], Quaternion.identity);
            PhotoData photodata = photoObject.GetComponent<PhotoData>();
            if (photodata != null)
            {
                photodata.SetPhotoName(photoName);
            }
        }
    }
    
    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EventType.eRaycasting:
                if (param is PlayerInteraction.RaycastInfo raycastInfo)
                {
                    if (raycastInfo.isHit)
                    {
                        hitObject = raycastInfo.hitInfo.collider.gameObject;
                        hitNetworkObject = hitObject.GetComponent<NetworkObject>();
                    }
                }
                
                break;
                
        }
        
    }

    public void OnDestroyPhoto()
    {
        if (hitNetworkObject != null)
        {
            var photo = hitNetworkObject.GetComponent<Photo>();
            photo.RpcDespawn();
        }
    }
}

