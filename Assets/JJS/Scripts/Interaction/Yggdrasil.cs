using UnityEngine;

// 광장 중앙 나무의 생태(?) 관할 클래스
// 해야 하는 업무
// 1. 로딩 되면 랜덤하게 서버에 저장된 사진/비디오 데이터를 불러온다
// -> 서버 연결 작업 전에는 일단 더미 데이터 불러오기 
// 2. 
public class Yggdrasil : MonoBehaviour
{
    //private MemoryGem[] _memoryGems = new MemoryGem[10];
    //private MemoryGemInfoSO[] _gemInfoArray; // TODO: 이게 맞냐
    //private const int MaxGemDisplayCount = 10; // TODO: 해보고 값 조정 필요

    [SerializeField] private MemoryGem[] memoryGems;

    private void Awake()
    {
        
    }
    
    private void Start()
    {
        
    }

    private void InitializeGems()
    {
        // 서버에서 저장된 영상의 목록을 가져오기
        // 그 중에서 랜덤하게 7개를 선택
        
        // 서버와 연결이 실패했을 경우 더미 데이터가 있는 서버에 접속
        // 열매에 할당
        foreach (var gem in memoryGems)
        {
            
        }
    }
}
