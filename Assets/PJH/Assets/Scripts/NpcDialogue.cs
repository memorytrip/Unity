using System;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public CanvasGroup dialogue;
    public GameObject exclamation;
    private TriggerDialogue td;
    
    
    private void Awake()
    {
        td = GetComponent<TriggerDialogue>();
    }
    
    void Update()
    {
        if (!td.CanTalk)
        {
            UIManager.HideUI(dialogue);
        }

        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
            
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        // 터치된 오브젝트 확인
                        Debug.Log($"Touched {hit.collider.gameObject.name}");

                        // 특정 오브젝트에 반응하도록 설정
                        if (hit.collider.CompareTag("Touchable"))
                        {
                            UIManager.ShowUI(dialogue);
                            exclamation.SetActive(false);
                        }
                    }
                }
            }
        }
    }

}