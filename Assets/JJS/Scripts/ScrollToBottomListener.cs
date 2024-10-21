using UnityEngine;
using UnityEngine.UI;

public class ScrollToBottomListener : MonoBehaviour
{
    public ScrollRect scrollRect;  // Reference to the ScrollRect
    public Button targetButton;    // Reference to the Button to be activated

    void Start()
    {
        // Ensure the button starts as disabled
        targetButton.interactable = false;
        
        // Add a listener to the ScrollRect's onValueChanged event
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    void OnScroll(Vector2 scrollPosition)
    {
        // Check if the ScrollRect has been scrolled to the bottom
        if (scrollRect.verticalNormalizedPosition <= 0.01f)
        {
            // Enable the button if scrolled to bottom
            targetButton.interactable = true;
        }
        else
        {
            // Keep the button disabled until scrolled to bottom
            targetButton.interactable = false;
        }
    }
}