using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class MapSelectionController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Button leftArrowButton; 
    public Button rightArrowButton;
    public float scrollDuration = 0.5f; 
    public int totalPages = 2; // 맵 페이지 수 -1 (아마 나중에 맵List.length 가 되어야 할 듯함)
    private float initialPosition;
    private int currentPage = 0;

    private void Start()
    {
        initialPosition = scrollRect.content.anchoredPosition.x;
        UpdateArrowButtons();
        scrollRect.content.sizeDelta = new Vector2(800 * totalPages, 700f);
        leftArrowButton.onClick.AddListener(PreviousPage);
        rightArrowButton.onClick.AddListener(NextPage);
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
        float targetHorizontalPosition = initialPosition - pageIndex * 800f;
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