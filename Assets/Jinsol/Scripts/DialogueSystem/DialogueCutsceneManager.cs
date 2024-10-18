using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueCutsceneManager : MonoBehaviour
{
    [SerializeField] private DialogueSO firstDialogueObject;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    private bool _choiceConfirmed;

    [SerializeField] private GameObject choiceUI;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private GameObject choiceParent; // TODO: FindParent 비슷한 거 있지 않았었나
    [SerializeField] private Transform choiceParentTransform;
    [SerializeField] private GameObject choiceButtonPrefab; // TODO: 선택지 프리팹 왜 만든건데ㅠㅠㅠㅠㅠㅠㅠ

    [SerializeField] private PlayableDirector director; // TODO: 이거 일일히 연결해야되냐

    private WaitForSeconds NextDialogueWait(DialogueSO.DialogueSegment dialogueObject)
    {
        return new WaitForSeconds(dialogueObject.displayTime);
    }
        
    // TODO: 이거 원래 firstDialogueObject를 기본값으로 넣어서 실행시켜주는 함수가 따로 있었는데 그렇게 하지 말고 어떻게든 해보기(??????)
    private void StartDialogue(DialogueSO dialogueObject)
    {
        StartCoroutine(DisplayDialogue(dialogueObject));
    }

    private IEnumerator DisplayDialogue(DialogueSO dialogueObject)
    {
        yield return null;
        List<GameObject> choiceButtons = new();
        choiceUI.SetActive(true);

        foreach (var dialogue in firstDialogueObject.dialogueSegments)
        {
            speechBubble.SetActive(true);
            speakerNameText.text = dialogue.speakerName;
            dialogueText.text = dialogue.dialogueText;

            if (dialogue.nextCutscene != null)
            {
                director.Play(dialogue.nextCutscene);
            }

            if (dialogue.choices.Count == 0)
            {
                Debug.Log($"Dialogue display time: {dialogue.displayTime}");
                yield return NextDialogueWait(dialogue);
            }
            else
            {
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
                yield return NextDialogueWait(dialogue);
                choiceParent.SetActive(true);

                foreach (var choice in dialogue.choices)
                {
                    var newButton = Instantiate(choiceButtonPrefab, choiceParentTransform);
                    choiceButtons.Add(newButton);
                    newButton.GetComponent<DialogueChoiceConnector>().Setup(this, choice.followupDialogue, choice.choiceText);
                }
                    
                speechBubble.SetActive(false);
                director.playableGraph.GetRootPlayable(0).SetSpeed(0);

                while (!_choiceConfirmed)
                {
                    yield return null;
                }

                break;
            }
        }
            
        choiceParent.SetActive(false);
        speechBubble.SetActive(false);
        choiceUI.SetActive(false);
        _choiceConfirmed = false;

        choiceButtons.ForEach(Destroy);
    }

    public void ChoiceSelected(DialogueSO selectedChoice)
    {
        _choiceConfirmed = true;
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        firstDialogueObject = selectedChoice;
        StartDialogue(selectedChoice);
    }
}