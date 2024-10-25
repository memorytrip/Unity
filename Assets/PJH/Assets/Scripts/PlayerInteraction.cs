using System;
using Fusion;
using UnityEngine;
using Random = System.Random;

public class PlayerInteraction : NetworkBehaviour
{
    public float interactionRange = 2.0f;
    public float interactionRadius = 0.5f;
    public RaycastHit hits;
    public LayerMask interactionLayer; 

    private void Awake()
    {
        interactionLayer = LayerMask.GetMask("Interactable");
    }

    void Update()
    {
        CheckForInteractable();
        PostNotice();
    }

    private bool CheckForInteractable()
    {
        // Physics.SphereCast는 bool 값을 반환하므로, hit에 bool이 들어가는 문제를 수정.
        return Physics.SphereCast(transform.position, interactionRadius, transform.forward, out hits, interactionRange, interactionLayer);
    }

    private void PostNotice()
    {
        if (HasStateAuthority)
        {
            if (CheckForInteractable())
            {
                {
                    EventManager.Instance.PostNotification(EventType.eRaycasting, this, new RaycastInfo(true, hits));
                }
            }
            else
            {
                EventManager.Instance.PostNotification(EventType.eRaycasting, this, new RaycastInfo(false, default));
            }
        }
    }
    public struct RaycastInfo
    {
        public bool isHit;
        public RaycastHit hitInfo;

        public RaycastInfo(bool isHit, RaycastHit hitInfo)
        {
            this.isHit = isHit;
            this.hitInfo = hitInfo;
        }
    }
    
}