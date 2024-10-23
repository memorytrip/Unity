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
    [Header("보상 내용 - 획득 시 팝업에 UI 표시")]
    [TextArea(15, 20)] public string rewardDescription; // TODO: 적절히 바꿔주기
    
    // TODO: 이거 왜 있는지 잊어버렸다
    private void OnValidate()
    {
#if UNITY_EDITOR
        RewardId = name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
