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
    private readonly Color _readyButtonColor = new(0.718f, 0.945f, 0.788f, 1f);
    private readonly Color _readyTextColor = new(0.183f, 0.18f, 0.887f, 1f);
    private readonly Color _startButtonColor = new(1f, 1f, 1f, 1f);
    private readonly Color _startTextColor = new(0.11f, 0.045f, 0.283f, 1f);

    [SerializeField] private Button exitButton;
    [SerializeField] public TMP_Text roomNameText;
    private GetLetterAndPhoto getLP;

    private const string AuthSuffix = " <sprite name=\"Auth\">";
    [SerializeField] private Image[] readyIcons;
    [SerializeField] private Sprite[] readyIconSprites;
    [SerializeField] private MapSelectionController mapSelectionController;
    
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
        while (true) {

            await UniTask.WaitUntil(() => Connection.list.Count == Runner.ActivePlayers.Count());
            var i = 0;
            readyCount = 0;
            foreach (var connection in Connection.list) {
                await UniTask.WaitUntil(() => (connection.currenctCharacter != null));
                //authMarks[i].alpha = connection.hasSceneAuthority ? 1f : 0f;
                string playerName = connection.playerName;
                playerNameTextList[i].text = connection.hasSceneAuthority ? playerName + AuthSuffix : playerName;
                
                bool isReady = connection.currenctCharacter.GetComponent<PlayReadyState>().ready;
                readyIcons[i].sprite = isReady ? readyIconSprites[1] : readyIconSprites[0];
                if (isReady) readyCount++;

                i++;
            }

            for (; i < playerNameTextList.Count; i++) {
                playerNameTextList[i].text = string.Empty;
                readyIcons[i].sprite = readyIconSprites[0];
            }

            if (Runner.IsSceneAuthority && readyCount == Connection.list.Count) {
                ActiveStart();
            }
            else
            {
                DeactiveStart();
            }
            
            await UniTask.Delay(500, cancellationToken: token);

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
        DeactiveByReady();
    }

    private void DeactiveByReady()
    {
        // readyButton.interactable = false;
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

    private void ActiveStart() {
        readyButtonImage.color = _startButtonColor; 
        readyButtonText.color = _startTextColor;
        readyButtonText.text = "시작하기";
        // readyButton.onClick.RemoveListener(Ready);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(RpcGameStart);
    }
    
    private void DeactiveStart() {
        readyButtonImage.color = _readyButtonColor;
        readyButtonText.color = _readyTextColor;
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
        _fmodTriggerOneshotManager.TriggerOneshot(playReadyConfirmEvent);
        getLP.GetResponse();
        cts.Cancel();
        LoadFindPhotoMap.maps = seletedMaps;
        foreach (var map in seletedMaps)
        {
            Debug.Log($"PlayReadyRoom.RpcGameStart: {map.Key} - {map.Value.mapId} {map.Value.mapType}");
        }
        SceneManager.Instance.MoveScene("FindPhoto");
    }
}
