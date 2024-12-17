using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Fusion;
using GUI;
using Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayReadyRoom : NetworkBehaviour, IStateAuthorityChanged
{
    [SerializeField] private List<TMP_Text> playerNameTextList;
    [SerializeField] private Button readyButton;
    private TextMeshProUGUI readyButtonText;
    private Image readyButtonImage;

    [SerializeField] private Button exitButton;
    [SerializeField] public TMP_Text roomNameText;
    private GetLetterAndPhoto getLP;

    private const string AuthSuffix = " <sprite name=\"Auth\">";
    [SerializeField] private Image[] readyIcons;
    [SerializeField] private Sprite[] readyIconSprites;
    [SerializeField] private MapSelectionController mapSelectionController;
    [SerializeField] private UploadPhoto uploadPhoto1;
    [SerializeField] private UploadPhoto uploadPhoto2;
    
    private int readyCount = 0;
    private CancellationTokenSource cts;
    
    Dictionary<string, MapId> seletedMaps = new Dictionary<string, MapId>();

    private FmodTriggerOneshotManager _fmodTriggerOneshotManager;
    [SerializeField] private EventReference playReadyConfirmEvent;

    private void Awake()
    {
        getLP = GetComponent<GetLetterAndPhoto>();
        readyButtonText = readyButton.GetComponentInChildren<TextMeshProUGUI>();
        readyButtonImage = readyButton.GetComponent<Image>();
        readyButton.interactable = false;
    }
    public override void Spawned()
    {
        _fmodTriggerOneshotManager = FindAnyObjectByType<FmodTriggerOneshotManager>();
        cts = new CancellationTokenSource();
        RefreshPlayerList(cts.Token).Forget();
        exitButton.onClick.AddListener(Exit);
        readyButton.onClick.AddListener(Ready);
        roomNameText.text = $"방 코드: {Runner.SessionInfo.Name}";
    }
    

    private async UniTaskVoid RefreshPlayerList(CancellationToken token)
    {
        while (true) 
        {
            await UniTask.WaitUntil(() => Connection.list.Count == Runner.ActivePlayers.Count());
            var i = 0;
            readyCount = 0;
            foreach (var connection in Connection.list) 
            {
                await UniTask.WaitUntil(() => (connection.currenctCharacter != null));
                //authMarks[i].alpha = connection.hasSceneAuthority ? 1f : 0f;
                string playerName = connection.playerName;
                playerNameTextList[i].text = connection.hasSceneAuthority ? playerName + AuthSuffix : playerName;
                
                bool isReady = connection.currenctCharacter.GetComponent<PlayReadyState>().ready;
                readyIcons[i].sprite = isReady ? readyIconSprites[1] : readyIconSprites[0];
                if (isReady) readyCount++;

                i++;
            }

            for (; i < playerNameTextList.Count; i++) 
            {
                playerNameTextList[i].text = string.Empty;
                readyIcons[i].sprite = readyIconSprites[0];
            }

            if (uploadPhoto1.photoId != 0 && uploadPhoto2.photoId != 0)
            {
                readyButton.interactable = true;
            }
            else
            {
                readyButton.interactable = false;
            }

            if (readyCount >= 2 && Runner.IsSceneAuthority && readyCount == Connection.list.Count) 
            {
                ActiveStart();
            }
            else
            {
                DeactiveStart();
            }
            
            await UniTask.Delay(100, cancellationToken: token);

            if (token.IsCancellationRequested) break;
        }
    }

    private void Ready()
    {
        foreach (var connection in Connection.list)
        {
            connection.currenctCharacter.GetComponent<PlayReadyState>().Ready();
        }
        string userId = SessionManager.Instance.currentUser.email;
        MapId mapId = mapSelectionController.GetSelectedMapId();
        RpcSendSelectedMap(userId, mapId.mapType == MapInfo.MapType.Custom, mapId.mapId);
        
        if (Connection.StateAuthInstance.currenctCharacter.GetComponent<PlayReadyState>().ready)
            DeactiveByReady();
        else 
            ActiveByUnready();
    }

    private void DeactiveByReady()
    {
        mapSelectionController.leftArrowButton.interactable = false;
        mapSelectionController.rightArrowButton.interactable = false;
        uploadPhoto1.selectPhotoButton.interactable = false;
        uploadPhoto2.selectPhotoButton.interactable = false;
    }

    private void ActiveByUnready()
    {
        mapSelectionController.leftArrowButton.interactable = true;
        mapSelectionController.rightArrowButton.interactable = true;
        uploadPhoto1.selectPhotoButton.interactable = true;
        uploadPhoto2.selectPhotoButton.interactable = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcSendSelectedMap(string name, bool isCustom, long mapId)
    {
        MapId map = new MapId(isCustom ? MapInfo.MapType.Custom : MapInfo.MapType.Default, mapId);
        if (!seletedMaps.ContainsKey(name))
            seletedMaps.Add(name, map);
        else
            seletedMaps[name] = map;
        Debug.Log($"PlayReadRoom.RpcSendSelectedMap: {name} - {map.mapId} ({map.mapType})");
    }

    private void ActiveStart()
    {
        readyButtonText.text = "시작하기";
        // readyButton.onClick.RemoveListener(Ready);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(RpcGameStart);
    }
    
    private void DeactiveStart() 
    {
        readyButtonText.text = "준비하기";
        // readyButton.onClick.RemoveListener(GameStart);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(Ready);
    }
    
    private void Exit()
    {
        cts.Cancel();
        SceneManager.Instance.MoveRoom(SceneName.Square).Forget();
    }

    public void StateAuthorityChanged()
    {
        exitButton.onClick.AddListener(Exit);
        readyButton.onClick.AddListener(Ready);
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcGameStart()
    {
        if (Runner.IsSceneAuthority)
        {
            var playerCount = Runner.ActivePlayers.Count();
            PhotoManager.TotalHiddenPhotoCount = 2 * playerCount;
        }
        
        _fmodTriggerOneshotManager.TriggerOneshot(playReadyConfirmEvent);
        getLP.GetResponse();
        cts.Cancel();
        
        List<string> playerIds = new List<string>();
        foreach (var connection in Connection.list)
        {
            playerIds.Add(connection.playerEmail);
        }
        LoadFindPhotoMap.playerIds = playerIds;
        LoadFindPhotoMap.maps = seletedMaps;
        
        foreach (var map in seletedMaps)
        {
            Debug.Log($"PlayReadyRoom.RpcGameStart: {map.Key} - {map.Value.mapId} {map.Value.mapType}");
        }
        SceneManager.Instance.MoveScene("FindPhoto");
    }
}