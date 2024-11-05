using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Clickable3dObjects : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayerMask;

    private void OnEnable()
    {
        Touch.onFingerDown += CheckMouseClick;
    }

    private void CheckMouseClick(Finger finger)
    {
        foreach (var touch in Touch.activeTouches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableLayerMask))
            {
                return;
            }
            
            var clickable = hit.collider.GetComponent<IClickable3dObject>();
            if (clickable != null)
            {
                clickable.OnClick();
            }
        }
    }
}