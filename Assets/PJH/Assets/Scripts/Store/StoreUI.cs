using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup storeUI;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject storeItemPrefab;

    private void Awake()
    {
        UIManager.HideUI(storeUI);
    }

    public void StoreOpen()
    {
        EventManager.Instance.OnPopupOpened();
        UIManager.ShowUI(storeUI);
        InitStoreUI().Forget();
    }

    public void StoreClose()
    {
        EventManager.Instance.OnPopupClosed();
        UIManager.HideUI(storeUI);
    }

    private async UniTaskVoid InitStoreUI()
    {
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
        
        string storeRawData = await DataManager.Get("/api/shop/items");
        StoreItemDTO[] storeData = JsonConvert.DeserializeObject<StoreItemDTO[]>(storeRawData);
        foreach (var data in storeData)
        {
            StoreListItem item = Instantiate(storeItemPrefab, content).GetComponent<StoreListItem>();
            item.SetName(data.name)
                .SetPrice(data.price)
                .SetImage(data.image);
        }        
    }
}

class StoreItemDTO
{
    public long id;
    public string name;
    public string image;
    public int price;
}