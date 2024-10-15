namespace Jinsol
{
    using UnityEngine;
    //using UnityEngine.Playables; TODO: 이것도 필요하기는 한데 (이하생략)
    using TMPro;
    using System.Collections;
    using System.Collections.Generic;

    public class DialogueDisplay : MonoBehaviour
    {
        private Queue<string> _sentences = new();
        [SerializeField] private TextMeshProUGUI speakerNameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [SerializeField] private NextDialogueButton nextDialogueButton; // TODO: 이거 아마 다른 방법을 할 수 있을 듯 (onClick)
        //[SerializeField] private PlayableDirector dialogueDirector; TODO: 선택적 감독

        private void Awake()
        {
            nextDialogueButton.standby = false;
        }

        private void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                nextDialogueButton.standby = true;
                return;
            }
            
            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        private IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                // TODO: 아마도 지금 이 사이에 typeInterval 넣어주면 될 듯...????? dksla akfrh
                yield return null;
            }
        }
    }
}