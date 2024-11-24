using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup storeUI;

    private void Awake()
    {
        UIManager.HideUI(storeUI);
    }

    public void StoreOpen()
    {
        EventManager.Instance.OnPopupOpened();
        UIManager.ShowUI(storeUI);
    }

    public void StoreClose()
    {
        EventManager.Instance.OnPopupClosed();
        UIManager.HideUI(storeUI);
    }
}