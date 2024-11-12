using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonWithRawImage : MonoBehaviour
{
    public int buttonID; 
    public UnityEvent<int> OnButtonClicked; 
    [HideInInspector] public RawImage childRawImage;

    void Start()
    {
        childRawImage = GetComponentInChildren<RawImage>();
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => NotifyCanvasController());
        }
    }

    private void NotifyCanvasController()
    {
        OnButtonClicked.Invoke(buttonID); 
    }
}