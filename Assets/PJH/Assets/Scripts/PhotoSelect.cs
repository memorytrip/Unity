using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using NUnit.Framework;

public class CanvasController : NetworkBehaviour
{
    public ButtonWithRawImage[] buttons;
    public List<RawImage> PhotoList = new List<RawImage>();
    public RawImage[] selectedPhotoArray;

    void Start()
    {
        foreach (var button in buttons)
        {
            button.OnButtonClicked.AddListener(OnButtonRawImageClicked);
        }
    }
    
    private void OnButtonRawImageClicked(int buttonID)
    {
        if (HasStateAuthority)
        {
            RpcAddPhotoToList(buttonID);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcAddPhotoToList(int buttonID)
    {
        RawImage rawImage = buttons[buttonID].childRawImage;
            if (rawImage != null)
            {
                if (!PhotoList.Contains(rawImage) && PhotoList.Count < 4)
                {
                    PhotoList.Add(rawImage);
                    Debug.Log("사진 추가");
                }
                else
                {
                    PhotoList.Remove(rawImage);
                    Debug.Log("사진 제거");
                }
            }
            else
            {
                Debug.LogWarning("RawImage가 없습니다.");
            }
            
            int maxIndex = Mathf.Min(selectedPhotoArray.Length, PhotoList.Count);
            Debug.Log(maxIndex);
            {
                for (int i = 0; i < maxIndex; i++)
                {
                    Debug.Log($"{i}번째 사진이 추가되었습니다.");
                    selectedPhotoArray[i].texture = PhotoList[i].texture;
                }

                for (int i = maxIndex; i < selectedPhotoArray.Length; i++)
                {
                    selectedPhotoArray[i].texture = null;
                }
            }
    }
    
    /*public void RpcAddPhotoToList(int buttonID)
    {
        AddPhotoToList(buttonID);
    }*/

}
