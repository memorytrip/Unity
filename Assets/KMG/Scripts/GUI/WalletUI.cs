using System;
using Common;
using Cysharp.Threading.Tasks;
using GUI;
using Newtonsoft.Json;
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
        InitWalletUI().Forget();
    }

    public void CloseWallet()
    {
        EventManager.Instance.OnPopupClosed();
        Utility.DisablePanel(canvasGroup);
    }

    private async UniTaskVoid InitWalletUI()
    {
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
        
        string storeRawData = await DataManager.Get("api/inventory");
        WalletItemDTO[] walletData = JsonConvert.DeserializeObject<WalletItemDTO[]>(storeRawData);
        foreach (var data in walletData)
        {
            WalletListItem item = Instantiate(walletItemPrefab, content).GetComponent<WalletListItem>();
            item.SetName(data.name)
                .SetImage(data.image);
        }
    }
}

class WalletItemDTO
{
    public string name;
    public string image;
    public int price;
}
