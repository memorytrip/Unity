using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Fusion;
using GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayReadyRoom : NetworkBehaviour, IStateAuthorityChanged
{
    [SerializeField] private List<TMP_Text> playerNameTextList;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button exitButton;
    [SerializeField] public TMP_Text roomNameText;
    [SerializeField] private GetLetterAndPhoto getLP;

    private const string AuthSuffix = " <sprite name=\"Auth\">";
    [SerializeField] private Image[] readyIcons;
    [SerializeField] private Sprite[] readyIconSprites;
    
    private int readyCount = 0;
    private CancellationTokenSource cts;

    private FmodTriggerOneshotManager _fmodTriggerOneshotManager;
    [SerializeField] private EventReference playReadyConfirmEvent;


    private void Awake()
    {
        getLP = GetComponent<GetLetterAndPhoto>();
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
        
        DeactiveByReady();
    }

    private void DeactiveByReady()
    {
        // readyButton.interactable = false;
    }

    private void ActiveStart() {
        readyButton.interactable = true;
        readyButton.GetComponentInChildren<TMP_Text>().text = "시작하기";
        // readyButton.onClick.RemoveListener(Ready);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(RpcGameStart);
    }
    
    private void DeactiveStart() {
        readyButton.interactable = true;
        readyButton.GetComponentInChildren<TMP_Text>().text = "준비하기";
        // readyButton.onClick.RemoveListener(GameStart);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(Ready);
        
    }
    
    private void Exit()
    {
        cts.Cancel();
        SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();
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
        SceneManager.Instance.MoveScene("FindPhoto");
    }
}
