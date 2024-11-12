using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Map;


public class MapSelectionController : MonoBehaviour
{
    public ScrollRect scrollRect;
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
        Debug.Log(totalPages);
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