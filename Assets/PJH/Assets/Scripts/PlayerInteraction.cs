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
    }

    public void CheckForInteractable()
    {
        // Physics.SphereCast는 bool 값을 반환하므로, hit에 bool이 들어가는 문제를 수정.
        bool isHit = Physics.SphereCast(transform.position, interactionRadius, transform.forward, out hits, interactionRange, interactionLayer);

        if (HasStateAuthority)
        {
            if (isHit)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 시작 위치에서 반경만큼의 구를 그려줍니다.
        Gizmos.DrawWireSphere(transform.position, interactionRadius);

        // SphereCast가 진행될 방향과 끝 위치를 계산합니다.
        Vector3 direction = transform.forward;
        Vector3 endPosition = transform.position + direction * interactionRange;

        // 구체 레이 시작 지점에서 끝 지점까지 레이를 그립니다.
        Gizmos.DrawLine(transform.position, endPosition);

        // 끝 위치에서 구체를 그려주어 SphereCast의 범위를 나타냅니다.
        Gizmos.DrawWireSphere(endPosition, interactionRadius);    }
}