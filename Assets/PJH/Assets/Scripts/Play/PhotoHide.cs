using System.Collections.Generic;
using System.Linq;
using Common.Network;
using Fusion;
using UnityEngine;

public class PhotoHide : NetworkBehaviour, IListener
{
    private float yValue = 1f;
    public GameObject photoPrefab;
    private GameObject hitObject;

    // void Start()
    public override void Spawned()
    {
        if (!Runner.IsSceneAuthority)
            return;
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
        SpawnPhoto(); 
    }
    
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10, 11), yValue, Random.Range(-10, 11));
    }

    public void SpawnPhoto()
    {
        foreach (var res in RecievedPhotoData.PhotoResponses)
        {
            var pos = GetRandomPosition(); 
            var spawnedPhoto = Runner.Spawn(photoPrefab, pos);
            var photo = spawnedPhoto.GetComponent<Photo>();
            if (photo != null)
                photo.Info = res;
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