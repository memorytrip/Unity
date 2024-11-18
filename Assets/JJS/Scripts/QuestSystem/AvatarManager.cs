using UnityEngine;
using System.Collections.Generic;

public class AvatarManager : MonoBehaviour
{
    private Dictionary<string, Renderer> headgearRenderers = new();
    private Dictionary<string, Renderer> eyewearRenderers = new();
    private Dictionary<string, Renderer> tShirtRenderers = new();
    private Dictionary<string, Renderer> equipmentRenderers = new();

    public void AddItem(Dictionary<string, Renderer> itemCategory, string itemId, Renderer itemRenderer)
    {
        itemCategory.TryAdd(itemId, itemRenderer);
    }

    public void ToggleItemVisibility(Dictionary<string, Renderer> itemCategory, string itemId, bool isVisible)
    {
        if (itemCategory.TryGetValue(itemId, out Renderer itemRenderer))
        {
            itemRenderer.enabled = isVisible;
        }
    }

    public void RemoveItem(Dictionary<string, Renderer> itemCategory, string itemId)
    {
        if (itemCategory.ContainsKey(itemId))
        {
            itemCategory.Remove(itemId);
        }
    }
}
