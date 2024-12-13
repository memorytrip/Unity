using Common;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreListItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;

    private int price;
    private string itemName;
    private string imageUri;

    public StoreListItem SetPrice(int price)
    {
        this.price = price;
        priceText.text = price.ToString();
        return this;
    }

    public StoreListItem SetName(string name)
    {
        this.itemName = name;
        nameText.text = name;
        return this;
    }

    public StoreListItem SetImage(string imageUri)
    {
        this.imageUri = imageUri;
        SetImageProcess(imageUri).Forget();
        return this;
    }

    private async UniTaskVoid SetImageProcess(string imageUri)
    {
        Sprite sprite = await DataManager.GetSprite(imageUri);
        if (sprite != null)
            image.sprite = sprite;
    }
}
