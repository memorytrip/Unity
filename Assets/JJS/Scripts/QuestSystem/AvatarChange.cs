using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarChange : NetworkBehaviour
{
    private AvatarManager _avatarManager;
    
    // TODO: 백엔드 연결해야하ㅏㅏㅏㅏ는데ㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔㅔ으악
    [SerializeField] private GameObject headgear;
    [SerializeField] private GameObject eyewear;
    [SerializeField] private GameObject tShirt;
    [SerializeField] private GameObject equipment;

    [SerializeField] private Transform head;
    [SerializeField] private Transform eyes;
    [SerializeField] private Transform upperBody;
    [SerializeField] private Transform hand;

    public override void Spawned()
    {
        if (SceneManager.GetActiveScene().name == "SpecialSquare" || SceneManager.GetActiveScene().name == "Square")
        {
            _avatarManager = FindAnyObjectByType<AvatarManager>();
        }
    }
    
    public void TakeItem(Dictionary<string, Renderer> itemCategory, string itemId, GameObject item)
    {
        var itemRenderer = item.GetComponent<Renderer>();
        if (itemRenderer != null)
        {
            _avatarManager.AddItem(itemCategory, itemId, itemRenderer);
        }
        Debug.Log("DOYAAAAAA");
    }

    public void WearItem(Dictionary<string, Renderer> itemCategory, string itemId, bool isVisible)
    {
        _avatarManager.ToggleItemVisibility(itemCategory, itemId, isVisible);
    }
}
