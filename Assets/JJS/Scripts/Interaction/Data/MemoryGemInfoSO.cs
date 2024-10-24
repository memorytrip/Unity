using UnityEngine;

// 나무에 맺힐 열매
public class MemoryGemInfoSO : ScriptableObject
{
    // 정보 불러올 때 GemType이 Photo면 photo가 null이면 안되고
    // GemType이 Video면 video가 null이면 안됨
    // 즉 남은 쪽은 null이어도 상관없음
    // 왜 적고 있는 건지 모르겠지만 일단 적어놓음 (????????)
    public enum GemType
    {
        Photo,
        Video,
    }

    public enum GemSize
    {
        Small = 1,
        Medium = 5,
        Large = 10,
    }
    
    public string memoryId; // 고유 ID
    public GemType gemType;
    public GemSize gemSize;
    public byte[] photo; // TODO: 이렇게 하는 게 맞나
    public byte[] video; // TODO: 이렇게 하는 게 맞나
}
