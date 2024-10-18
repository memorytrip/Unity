using UnityEngine;

// 자동으로 모든 퀘스트가 활성화
// 퀘스트 목록은 메인 메뉴에서 확인 가능
// 퀘스트마다 보상이 있을 수도 있고 없을 수도 있음 (보상이 없는 퀘스트는 최종 특전 제공의 조건 체크용)
// 퀘스트 전부 달성 시 특전 제공
// 따로 수락하는 퀘스트가 없기 때문에 복잡하게 설계하지 않아도 될 듯 (아마도)
//
// <퀘스트 매니저가 해야 할 일>
// - 전체 퀘스트 파악 (시작 시 1회)
// - 개별 퀘스트 달성 파악 (퀘스트가 달성 되었을 때마다)
// - 최종 보상을 위한 조건 충족 체크 (퀘스트가 달성 되었을 때마다)
//
// <TODO: 백엔드와 상의해야 할 것>
// - 전체 퀘스트 갯수가 정해져 있음을 전달
// - 유저별로 퀘스트 달성 현황(개별 퀘스트의 달성 여부, 중복 보상 수령 방지를 위한 개별 및 전체 보상 획득 여부)을 서버에 저장
// - 위 내용의 '초기값(미달성 상태인 모든 퀘스트 내용)' 또한 서버에 저장해서 불러와야 함
// - 보상의 내용은 아직 미정이나 현재는 쿠폰, 크레딧(후순위), 아바타 스킨 (최후순위) 예상 중
public class QuestManager : MonoBehaviour
{
    private Draft.Quest[] _quests; // TODO: 퀘스트 몇개까지 할 지 미정: _maxQuestNumber 조정 필요(프로토 타입에서는 7개 정도로 하기)
    private const int MaxQuestNumber = 7;
    private int _clearedQuests;
    
    private void Awake()
    {
        _quests = new Draft.Quest[MaxQuestNumber];
    }

    private void Start()
    {
        InitializeQuests();
        LoadQuestStatus();
    }

    // 유저 최초 접속 시에는 맵에 있는 모든 퀘스트를 불러옴 - TODO: 사실 이것도 궁극적으로 백엔드에 초기값을 저장해서 불러와야 함
    private void InitializeQuests()
    {
        
    }

    // 유저 접속이 초회가 아닌 경우 저장된 퀘스트 상황을 불러옴
    private void LoadQuestStatus()
    {
        // 백엔드에서 불러와서 반영시키는 로직
    }

    private bool QuestAllClear()
    {
        // 퀘스트가 달성된 이벤트를 구독하고 있다가 그때마다 전체 퀘스트 달성 여부 체크
        return _clearedQuests == MaxQuestNumber;
    }
    
    private void ClearQuest(Draft.Quest quest)
    {
        quest.ClearQuest();
        _clearedQuests++;
    }
}
