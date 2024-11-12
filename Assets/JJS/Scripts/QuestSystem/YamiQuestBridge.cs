using System.Collections;
using GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YamiQuestBridge : MonoBehaviour
{
    [SerializeField] private CanvasGroup questPopup;
    private FadeController _fadeController;
    private readonly WaitForSeconds _displayTime = new(1.5f); 
    
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI message;
    
    private static readonly string[] Titles = { "퀘스트 달성!", "스탬프 완주!" }; // enum QuestState

    private void Awake()
    {
        _fadeController = questPopup.GetComponent<FadeController>();
    }
    
    public void SetData(bool allClear, string questNameText, Sprite rewardImageSprite, string messageText)
    {
        title.text = allClear ? Titles[1] : Titles[0];
        questName.text = questNameText;
        rewardImage.sprite = rewardImageSprite;
        message.text = messageText;
    }

    public void ShowUi(bool stayOpen)
    {
        StartCoroutine(ProcessShowUi(stayOpen));
    }
    
    private IEnumerator ProcessShowUi(bool stayOpen)
    {
        _fadeController.StartFadeIn(0.1f);
        if (stayOpen)
        {
            yield break;
        }
        yield return _displayTime;
        HideUi();
    }

    public void HideUi()
    {
        _fadeController.StartFadeOut(0.5f);
    }
}