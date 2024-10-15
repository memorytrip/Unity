using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables; // TODO: ???
    
[RequireComponent(typeof(Collider))]
public class QuestPoint : MonoBehaviour
{
    [Header("담당 퀘스트")] [SerializeField] private QuestInfoSO questInfo;
    private string _questId;
    private EQuestState _currentQuestState;
    //private QuestIcon _questIcon;

    [Header("퀘스트 단계 설정")]
    // 시작 퀘스트 : 퀘스트 수주 단계
    // 완료 퀘스트 : 퀘스트 완료 단계
    [Tooltip("시작 퀘스트인지?")] [SerializeField] private bool startPoint;
    [Tooltip("완료 퀘스트인지?")][SerializeField] private bool finishPoint;

    [SerializeField] private Transform questPointPosition;
    private Transform _player;
    private bool _playerNearby;
    private EventTrigger _eventTrigger;

    // TODO: 없으면 생략
    [Header("우측 퀘스트 목록 갱신용 파라미터")] [SerializeField]
    private Transform questList; // TODO: 이름 다시 정하기

    // TODO: 이거 아무리 봐도 급해서 GameObject 남발한 거 같은데 자료형 재검토
    [SerializeField] private GameObject questTitlePrefab;
    [SerializeField] private GameObject questStepPrefab;
    [SerializeField] private List<GameObject> questTitleList = new();
    [SerializeField] private List<GameObject> questStepList = new();
        
    // TODO: 퀘스트마다 연출이 필요한 게 있고 필요없는 게 있어서 사실 여기에 있으면 안 될 것 같음
    [SerializeField] private PlayableDirector questCutsceneDirector;
    [SerializeField] private PlayableAsset startQuestCutscene;
    [SerializeField] private PlayableAsset[] interQuestCutscenes; // TODO: 급조한 거라 다시 검토
    [SerializeField] private PlayableAsset finishQuestCutscene;

    private void Awake()
    {
        _questId = questInfo.questId;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        //questIcon = GetComponentInChildren<QuestIcon>();
        _eventTrigger = GetComponentInChildren<EventTrigger>();
    }
}