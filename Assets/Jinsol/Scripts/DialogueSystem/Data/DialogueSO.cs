namespace Jinsol
{
    using UnityEngine;
    using UnityEngine.Playables; //TODO: 필요하긴 한데 항상 있어야 하는 건 아니라서 고민중
    using System;
    using System.Collections.Generic;

    [Serializable]
    [CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/NPC_Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [Serializable]
        public struct DialogueSegment
        {
            public string speakerName;
            public string dialogueText;
            public float displayTime;
            public PlayableAsset nextCutscene;
            public List<DialogueChoice> choices;
        }

        [Serializable]
        public struct DialogueChoice
        {
            public string choiceText;
            public DialogueSO followupDialogue;
        }
        
        public List<DialogueSegment> dialogueSegments = new();
        [SerializeField] private DialogueSO followupDialogue; //TODO: endDialogue라고 적었었는데 모르겠다
    }
}