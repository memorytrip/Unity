using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleCanvasGroup : MonoBehaviour, IToggleStateListener
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        Toggle();
    }
    
    public void Toggle()
    {
        if (SceneManager.GetActiveScene().name == "Square")
        {
            _canvasGroup.alpha = ScreenshotManager.CameraMode == ECameraMode.Default ? 1f : 0f;
        }
    }
}
