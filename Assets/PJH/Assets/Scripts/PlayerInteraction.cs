using System;
using UnityEngine;
using Random = System.Random;

public class PlayerInteraction : MonoBehaviour
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
    }

    public void CheckForInteractable()
    {
        // Physics.SphereCast는 bool 값을 반환하므로, hit에 bool이 들어가는 문제를 수정.
        bool isHit = Physics.SphereCast(transform.position, interactionRadius, transform.forward, out hits, interactionRange, interactionLayer);
        
        if (isHit)
        {
            // 오브젝트가 감지된 경우 이벤트를 통해 감지 사실을 알림.
            EventManager.Instance.PostNotification(EventType.eRaycasting, this, new RaycastInfo(true, hits));
        }
        else
        {
            // 감지된 오브젝트가 없을 때 이벤트로 알림.
            EventManager.Instance.PostNotification(EventType.eRaycasting, this, new RaycastInfo(false, default));
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