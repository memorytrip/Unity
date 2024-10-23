using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

// 서버 연결까지 생각하면 복잡할 것 같아서 일단은 연출이라도 제대로 하기

// TODO: 나무 연출
// 플레이어가 접근하면 기존에 나무에 달린 열매들의 비디오/사진이 사라지고 (열매 모델만 남음)
// 열람 가능한 열매가 화면 상단에서 중앙으로 슬라이드
// 클릭하면 상세 정보 볼 수 있는 UI 팝업
// 엽서일 경우 엽서 전용 UI
// 영상일 경우 영상 전용 UI
public class ToggleYggdrasil : MonoBehaviour
{
    private static readonly int Exit = Animator.StringToHash("Exit");
    private Yggdrasil _yggdrasil;
    
    // 불러올 열매들을 저장하는 배열
    private MemoryGem[] _memoryGemSet;
    [FormerlySerializedAs("backgroundMemoryGemSet")] [SerializeField] private VideoPlayer[] backgroundMemoryGemSetVideo = new VideoPlayer [10]; // 이거는 그냥 서버에서 이미지나 비디오만 가져올 것
    [SerializeField] private RawImage[] backgroundMemoryGemSetImage;
    private const int MaxGems = 7; // TODO: 변동 가능. 일단은 길이를 7로 잡기
    [SerializeField] private GameObject content;
    private Animator _contentUiAnimator;
    private readonly WaitForSeconds _exitClipLength = new(1.5f);
    [SerializeField] private CinemachineCamera yggdrasilCamera;
    [SerializeField] private CinemachineCamera defaultCamera;

    private void Awake()
    {
        _yggdrasil = GetComponent<Yggdrasil>();
        _memoryGemSet = new MemoryGem[MaxGems];
        _memoryGemSet = FindObjectsByType<MemoryGem>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        ActivateButtons(_memoryGemSet);
        _contentUiAnimator = content.GetComponentInChildren<Animator>(includeInactive:true);
        if (_contentUiAnimator == null)
        {
            Debug.LogWarning("Animator is not initialized!");
        }
        
        defaultCamera.Prioritize();
        //yggdrasilCamera.SetActive(false);
        content.SetActive(false);
    }
    
    private void Start()
    {
        StartCoroutine(LoadGems());
    }

    private IEnumerator LoadGems()
    {
        // TODO: 서버로부터 불러오는 로직
        yield return null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        StartCoroutine(ViewGems());
        Debug.Log($"Player entered {gameObject}");
    }

    private IEnumerator ViewGems()
    {
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Player");
        foreach (var gem in backgroundMemoryGemSetVideo)
        {
            gem.Stop();
        }
        content.SetActive(true);
        yggdrasilCamera.Prioritize();
        Debug.Log($"Yggdrasil camera is now {yggdrasilCamera.gameObject}, Priority: {yggdrasilCamera.Priority}");
        //yggdrasilCamera.SetActive(true);
        DeactivateBackgroundGems();
        // 열람 가능한 열매가 화면 상단에서 중앙으로 슬라이드
        // 이 열매는 항상 개수가 고정되어 있다
        // 크기는 다양할 수 있음 - 불러오는 열매 정보(GemInfoSO)에 기재된 크기에 따라 달라짐 (이게 맞나)
        yield return null;
    }

    private void DeactivateBackgroundGems()
    {
        
    }

    private void ActivateButtons(MemoryGem[] memoryGemSet)
    {
        foreach (var memoryGem in memoryGemSet)
        {
            //memoryGem.GetComponent<Button>().onClick.AddListener(DisplayContentUi);
        }
    }

    public void DisplayContentUi()
    {
        Debug.Log("Tap!");
        //MemoryGem selectedMemoryGem = EventSystem.current.currentSelectedGameObject.GetComponent<MemoryGem>();
        //ToggleRevealContent(selectedMemoryGem);
    }

    private void ToggleRevealContent(MemoryGem memoryGem)
    {
    }

    private IEnumerator OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            yield break;
        }

        _contentUiAnimator.SetTrigger(Exit);
        yield return _exitClipLength;
        HideContentUi();
        foreach (var gem in backgroundMemoryGemSetVideo)
        {
            gem.Play();
        }
        Camera.main.cullingMask = -1; 
        Debug.Log($"Player exited {gameObject}");
    }

    private void HideContentUi()
    {
        content.SetActive(false);
        defaultCamera.Prioritize();
    }

    /*private void DeactivateButtons(MemoryGem[] memoryGemSet)
    {
        foreach (var memoryGem in memoryGemSet)
        {
            memoryGem.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }*/

    /*private void OnDisable()
    {
        DeactivateButtons(_memoryGemSet);
    }*/
}