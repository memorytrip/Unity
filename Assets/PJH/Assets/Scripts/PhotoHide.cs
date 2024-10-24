using System.Collections.Generic;
using Common.Network;
using Fusion;
using UnityEngine;

public class PhotoHide : NetworkBehaviour, IListener
{
    private float yValue = 1f;
    private Dictionary<string, Vector3> photoPositions;
    public GameObject photoPrefab;
    private GameObject hitObject;

    // void Start()
    public override void Spawned()
    {
        if (!Runner.IsSceneAuthority)
            return;
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
        photoPositions = new Dictionary<string, Vector3>();
        HidePhoto(photoPositions, 8); 
    }
    
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10, 11), yValue, Random.Range(-10, 11));
    }

    public void HidePhoto(Dictionary<string, Vector3> photoDict, int numberOfPhotos)
    {
        for (int i = 1; i <= numberOfPhotos; i++)
        {
            string photoName = "Photo" + i;
            photoDict[photoName] = GetRandomPosition(); 
            // Instantiate(photoPrefab, photoDict[photoName], Quaternion.identity);
            Runner.Spawn(photoPrefab, photoDict[photoName]);
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
                    }
                }

                break;
                
        }
        
    }

    public void DestroyPhoto()
    {
        Destroy(hitObject);
    }
}