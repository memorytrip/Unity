using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Map;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class MapSelectionController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollContent;
    public GameObject mapThumbnailPrefab;
    public Sprite EmptySprite;
    public Button leftArrowButton; 
    public Button rightArrowButton;
    public float scrollDuration = 0.5f; 
    public int totalPages = 0; // 맵 페이지 수 -1 (아마 나중에 맵List.length 가 되어야 할 듯함)
    private float initialPosition;
    private int currentPage = 0;

    private void Start()
    {
        initialPosition = scrollRect.content.anchoredPosition.x;
        UpdateArrowButtons();
        //scrollRect.content.sizeDelta = new Vector2(800 * totalPages, 700f);
        leftArrowButton.onClick.AddListener(PreviousPage);
        rightArrowButton.onClick.AddListener(NextPage);
        
        InitMapList().Forget();
    }

    private async UniTaskVoid InitMapList()
    {
        User user = SessionManager.Instance.currentUser;
        List<MapInfo> mapList = await MapManager.Instance.LoadMapList(user);

        List<UniTask> tasks = new List<UniTask>(mapList.Count);
        foreach (var mapInfo in mapList)
        {
            tasks.Add(LoadMapThumbnail(mapInfo));
        }
        await UniTask.WhenAll(tasks);
        
        totalPages = mapList.Count;
        UpdateArrowButtons();
    }

    private async UniTask LoadMapThumbnail(MapInfo mapInfo)
    {
        Image image = Instantiate(mapThumbnailPrefab, scrollContent).GetComponent<Image>();

        if (Uri.TryCreate(mapInfo.thumbnail, UriKind.Absolute, out Uri uri))
        {
            image.sprite = await LoadMapThumbnailFromServer(mapInfo.thumbnail);
        }
        else if (File.Exists(mapInfo.thumbnail))
        {
            image.sprite = await LoadMapThumbnailFromLocal(mapInfo.thumbnail);
        }
        else
        {
            image.sprite = EmptySprite;
        }
    }

    private async UniTask<Sprite> LoadMapThumbnailFromServer(string uri)
    {
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerTexture();
        
        Sprite sprite;
        
        try
        {
            await request.SendWebRequest();
        }
        catch (UnityWebRequestException e)
        {
            return EmptySprite;
        }
        finally
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                sprite = EmptySprite;
            }
        }
        return sprite;
    }

    private async UniTask<Sprite> LoadMapThumbnailFromLocal(string path)
    {
        if (File.Exists(path))
        {
            byte[] rawdata = await DataManager.Read(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(rawdata))
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
        return EmptySprite;
    }

    void NextPage()
    {
        Debug.Log("오른쪽 버튼");
        if (currentPage < totalPages -1)
        {
            currentPage++;
            ScrollToPage(currentPage);
        }
    }

    void PreviousPage()
    {
        Debug.Log("왼쪽 버튼");
        if (currentPage > 0)
        {
            currentPage--;
            ScrollToPage(currentPage);
        }
    }

    private void ScrollToPage(int pageIndex)
    {
        float targetHorizontalPosition = initialPosition - pageIndex * 450f;
        Debug.Log(targetHorizontalPosition);
        scrollRect.content.DOAnchorPosX(targetHorizontalPosition, scrollDuration);
        
        UpdateArrowButtons();
    }

    private void UpdateArrowButtons()
    {
        leftArrowButton.interactable = currentPage > 0;
        rightArrowButton.interactable = currentPage < totalPages - 1;
    }
}   