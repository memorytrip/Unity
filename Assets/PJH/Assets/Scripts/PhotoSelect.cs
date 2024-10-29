using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Common;
using Fusion;
using NUnit.Framework;

public class PhotoSelect : NetworkBehaviour
{
    public ButtonWithRawImage[] buttons;
    public List<RawImage> PhotoList = new List<RawImage>();
    public RawImage[] selectedPhotoArray;
    public Button submitButton;

    void Start()
    {
        submitButton.gameObject.SetActive(false);
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
        Debug.Log($"포토 리스트: {PhotoList.Count}");
            if (rawImage != null)
            {
                if (PhotoList.Count <= 4)
                {
                    submitButton.gameObject.SetActive(false);
                    if (!PhotoList.Contains(rawImage))
                    {
                        PhotoList.Add(rawImage);
                        Debug.Log("사진 추가");
                    }
                    else
                    {
                        PhotoList.Remove(rawImage);
                        Debug.Log("사진 제거");
                    }

                    if (PhotoList.Count == 4 && HasStateAuthority)
                    {
                        submitButton.gameObject.SetActive(true);
                        submitButton.onClick.AddListener(()=>RpcSceneChange("VideoLoadTest"));
                    }
                }
                
            }
            else
            {
                Debug.LogWarning("RawImage가 없습니다.");
            }
            
            int maxIndex = Mathf.Min(selectedPhotoArray.Length, PhotoList.Count);
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
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RpcSceneChange(string scenename)
    {
        SceneManager.Instance.MoveScene(scenename);
    }
}
