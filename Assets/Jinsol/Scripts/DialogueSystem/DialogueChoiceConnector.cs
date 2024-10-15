using UnityEngine;
using TMPro;
    
// 생성된 선택지 프리팹에 필요한 컴포넌트를 연결 TODO: 이게 맞냐
public class DialogueChoiceConnector : MonoBehaviour
{
    private DialogueCutsceneManager _manager;
    private DialogueSO _dialogueObject;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public void Setup(DialogueCutsceneManager manager, DialogueSO dialogueObject, string text)
    {
        _manager = manager;
        _dialogueObject = dialogueObject;
        dialogueText.text = text;
    }
        
    // TODO: 이게 최선이냐
    public void SelectChoice()
    {
        _manager.ChoiceSelected(_dialogueObject);
    }
}