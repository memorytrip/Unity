using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2.0f; // 상호작용 가능한 범위
    public LayerMask interactionLayer; // interaction 레이어 설정
    public KeyCode interactKey = KeyCode.E; // 상호작용 키 (E)
    private IInteractable currentInteractable;

    private void Awake()
    {
        interactionLayer = LayerMask.NameToLayer("Interactable");
        
    }
    

    void Update()
    {
        CheckForInteractable();
        
        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractable()
    {
        // 플레이어 앞에 구체 모양의 Raycast를 쏨
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward, interactionRange, interactionLayer);

        if (hitColliders.Length > 0)
        {
            currentInteractable = hitColliders[0].GetComponent<IInteractable>();
            if (currentInteractable != null)
            {
                // E 버튼 활성화 로직을 여기서 처리
                Debug.Log("상호작용 가능한 물체 발견! E 버튼을 누르세요.");
            }
        }
        else
        {
            currentInteractable = null;
            // E 버튼 비활성화 로직
        }
    }

    void OnDrawGizmos()
    {
        // 디버깅을 위해 Sphere의 범위를 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward, interactionRange);
    }
}