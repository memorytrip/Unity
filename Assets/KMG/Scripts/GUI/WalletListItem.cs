using Common;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletListItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;
    
    private string itemName;
    private string imageUri;
    
    public WalletListItem SetName(string name)
    {
        this.itemName = name;
        nameText.text = name;
        return this;
    }

    public WalletListItem SetImage(string imageUri)
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
