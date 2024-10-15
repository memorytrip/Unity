using UnityEngine.Serialization;

namespace Jinsol
{
    using UnityEngine;
    using UnityEngine.Playables;

    // TODO: 이거 아마 다른 방법 써도 될 것 같은데?ㅠㅠㅠㅠㅠ
    public class NextDialogueButton : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director; // TODO: 선택적 감독
        [SerializeField] private PlayableAsset nextPlayableAsset;
        public bool standby; //TODO: ㅋㅋ큐ㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠ

        // TODO: 머리아픔
        private void Update()
        {
            if (standby)
            {
                //director.extrapolationMode = DirectorWrapMode.None;
                Debug.Log($"Director stopped");
                //director.Play(nextPlayableAsset);
                Debug.Log($"Playing next playable asset...");
            }
        }
    }
}