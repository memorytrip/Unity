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
        UIManager.ShowUI(storeUI);
    }

    public void StoreClose()
    {
        UIManager.HideUI(storeUI);
    }
}
