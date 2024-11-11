using UnityEngine;

public class ToggleCanvasGroup : MonoBehaviour, IToggleStateListener
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Toggle()
    {
        _canvasGroup.alpha = ScreenshotManager.CameraMode == ECameraMode.Default ? 1f : 0f;
    }
}