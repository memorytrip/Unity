using UnityEngine;
using UnityEngine.InputSystem;

public class Clickable3dObjects : MonoBehaviour
{
    // LayerMask to limit raycast to specific layers, if desired
    [SerializeField] private LayerMask clickableLayerMask;

    void Update()
    {
        // Check for mouse click
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
}