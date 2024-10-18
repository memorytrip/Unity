// PJH

namespace JinsolTest.PJH
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    // 테스트용 카피본. 사용이 확정될 시 원작성자 코드로 교체 예정
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
}