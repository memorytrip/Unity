using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Common;
using Fusion;

    
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

    private void ActiveSceneChangeButton()
    {
        submitButton.gameObject.SetActive(false);

        if (PhotoList.Count == 4 && HasStateAuthority)
        {
            submitButton.gameObject.SetActive(true);
            submitButton.onClick.AddListener(() => RpcSceneChange("LetterScene"));
        }

    }

    private void ShowPhotoList()
    {
        int maxIndex = Mathf.Min(selectedPhotoArray.Length, PhotoList.Count);
        {
            for (int i = 0; i < maxIndex; i++)
            {
                selectedPhotoArray[i].texture = PhotoList[i].texture;
                selectedPhotoArray[i].color = new Color(1, 1, 1, 1);
            }

            for (int i = maxIndex; i < selectedPhotoArray.Length; i++)
            {
                selectedPhotoArray[i].texture = null;
                selectedPhotoArray[i].color = new Color(0.165f, 0.149f, 0.149f, 1);
            }
        }
    }
    
    public void AddPhoto(List<RawImage> photoList, int limit, RawImage rawImage)
    {
        if (photoList.Count > limit)
            return;

        if (!photoList.Contains(rawImage))
        {
            photoList.Add(rawImage);
            Debug.Log("사진 추가");
        }
        else
        {
            photoList.Remove(rawImage);
            Debug.Log("사진 제거");
        }
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcAddPhotoToList(int buttonID)
    {
        RawImage rawImage = buttons[buttonID].childRawImage;
        Debug.Log($"포토 리스트: {PhotoList.Count}");
        if (rawImage == null)
        {
            Debug.LogWarning("RawImage가 없습니다.");
            return;
        }

        AddPhoto(PhotoList, 4, rawImage);
        ActiveSceneChangeButton();
        ShowPhotoList();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcSceneChange(string scenename)
    {
        SceneManager.Instance.MoveScene(scenename);
    }
}
