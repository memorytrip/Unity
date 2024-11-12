using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using GUI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayReadyRoom : NetworkBehaviour, IStateAuthorityChanged
{
    [SerializeField] private List<TMP_Text> playerNameTextList;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text roomNameText;
    private int readyCount = 0;
    private CancellationTokenSource cts;
    public override void Spawned()
    {
        cts = new CancellationTokenSource();
        RefreshPlayerList(cts.Token).Forget();
        exitButton.onClick.AddListener(Exit);
        readyButton.onClick.AddListener(Ready);
        roomNameText.text = $"Room: {Runner.SessionInfo.Name}";
    }
    

    private async UniTaskVoid RefreshPlayerList(CancellationToken token)
    {
        while (true) {

            await UniTask.WaitUntil(() => Connection.list.Count == Runner.ActivePlayers.Count());
            var i = 0;
            readyCount = 0;
            foreach (var connection in Connection.list) {
                await UniTask.WaitUntil(() => (connection.currenctCharacter != null));
                string sceneAuthPrefix = (connection.hasSceneAuthority ? "* " : "");
                string playerName = connection.playerName;
                string ready;
                if (connection.currenctCharacter.GetComponent<PlayReadyState>().ready) {
                    ready = "O";
                    ++readyCount;
                } else {
                    ready = "X";
                }
                playerNameTextList[i++].text = $"{sceneAuthPrefix}{playerName} [{ready}]";
            }

            for (; i < playerNameTextList.Count; i++) {
                playerNameTextList[i].text = "-";
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
        readyButton.GetComponentInChildren<TMP_Text>().text = "Start";
        // readyButton.onClick.RemoveListener(Ready);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(RpcGameStart);
    }
    
    private void DeactiveStart() {
        readyButton.interactable = true;
        readyButton.GetComponentInChildren<TMP_Text>().text = "Ready";
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
        Debug.Log($"sachanged");
        exitButton.onClick.AddListener(Exit);
        readyButton.onClick.AddListener(Ready);
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcGameStart()
    {
        Debug.Log("start game");
        cts.Cancel();
        SceneManager.Instance.MoveScene("MultiPlayTest");
    }
}
