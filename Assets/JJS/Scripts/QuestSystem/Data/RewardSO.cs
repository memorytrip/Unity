using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Reward", menuName = "QuestSystem/Reward")]
public class RewardSO : ScriptableObject
{
    public enum RewardType
    {
        Coupon,
        Credit,
        AvatarSkin,
    }
    
    public string RewardId { get; private set; }
    public RewardType rewardType;
    [Header("보상 이름 - 획득 시 팝업에 UI 표시")]
    public string rewardName;
    [Space(10f)]
    [Header("보상 미리보기 이미지")]
    public Sprite rewardSprite;
    [Header("UI가 있을 경우 선택했을 때 풍선 띄우기")] // TODO: 와우
    [TextArea(15, 20)] public string rewardDescription;
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        RewardId = name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
