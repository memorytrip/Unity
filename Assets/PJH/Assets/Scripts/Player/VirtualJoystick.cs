using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    private float leverRange = 100;

    public Vector2 inputDirection;
    public bool isInput;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        LeverController(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        LeverController(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
    }

    private void LeverController(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputMaxPos = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputMaxPos; 
        inputDirection = inputMaxPos / leverRange;
    }
}
