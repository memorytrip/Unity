using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Map;
using Common.Network;


public class MapSelectionController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollContent;
    public GameObject mapThumbnailPrefab;
    public Sprite EmptySprite;
    public Button leftArrowButton; 
    public Button rightArrowButton;
    public float scrollDuration = 0.5f;
    private int totalPages;
    private float initialPosition;
    private int currentPage = 0;

    private async UniTask Start()
    {
        List<MapInfo> mapList = await MapManager.Instance.LoadMapList(new User()); //user정보에서 맵갯수 다운받아야 함
        totalPages = mapList.Count;
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
        image.sprite = await MapManager.Instance.LoadMapThumbnail(mapInfo);
        if (image.sprite == null)
            image.sprite = EmptySprite;
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