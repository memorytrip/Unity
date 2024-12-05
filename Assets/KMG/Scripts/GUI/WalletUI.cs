using System;
using GUI;
using UnityEngine;

public class WalletUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject walletItemPrefab;

    private void Awake()
    {
        Utility.DisablePanel(canvasGroup);
    }
    
    public void OpenWallet()
    {
        EventManager.Instance.OnPopupOpened();
        Utility.EnablePanel(canvasGroup);
    }

    public void CloseWallet()
    {
        EventManager.Instance.OnPopupClosed();
        Utility.DisablePanel(canvasGroup);
    }
}
