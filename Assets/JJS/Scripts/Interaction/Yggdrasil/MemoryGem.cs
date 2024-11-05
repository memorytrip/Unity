using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Button))]
public class MemoryGem : MonoBehaviour, IClickable3dObject
{
    public void OnClick()
    {
        Debug.Log("OnClick");
    }
}