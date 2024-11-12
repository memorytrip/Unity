using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Clickable3dObjects : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayerMask;

    private void OnEnable()
    {
        #if UNITY_ANDROID
        Touch.onFingerDown += CheckMouseClick;
        #endif
    }

    private void Update()
    {
        #if UNITY_EDITOR
        CheckMouseClick();
        #endif
    }
    
    private void CheckMouseClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            // Check if the ray hits any object in the scene
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableLayerMask))
            {
                // If the hit object has a specific component, interact with it
                var clickable = hit.collider.GetComponent<IClickable3dObject>();
                if (clickable != null)
                {
                    clickable.OnClick();
                }
                else
                {
                    Debug.Log("3D Object clicked: " + hit.collider.name);
                }
            }
        }
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