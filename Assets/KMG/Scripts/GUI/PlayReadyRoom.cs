using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using GUI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayReadyRoom : NetworkBehaviour, IPlayerJoined, IPlayerLeft, IStateAuthorityChanged
{
    [SerializeField] private List<TMP_Text> playerNameTextList;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text roomNameText;
    private int readyCount = 0;
    
    public override void Spawned()
    {
        RefreshPlayerList().Forget();
        exitButton.onClick.AddListener(Exit);
        readyButton.onClick.AddListener(Ready);
        roomNameText.text = $"Room: {Runner.SessionInfo.Name}";
    }
    
    public void PlayerJoined(PlayerRef player)
    {
        RefreshPlayerList().Forget();
    }

    public void PlayerLeft(PlayerRef player)
    {
        RefreshPlayerList().Forget();
    }

    private async UniTaskVoid RefreshPlayerList()
    {
        await UniTask.WaitUntil(() => Connection.list.Count == Runner.ActivePlayers.Count());
        var i = 0;
        foreach (var connection in Connection.list)
        {
            await UniTask.WaitUntil(() => (connection.currenctCharacter != null));
            string sceneAuthPrefix = (connection.hasSceneAuthority ? "* " : "");
            string playerName = connection.playerName;
            string ready = connection.currenctCharacter.GetComponent<PlayReadyState>().ready ? "O" : "X";
            playerNameTextList[i++].text = $"{sceneAuthPrefix}{playerName} [{ready}]";
        }

        for (; i < playerNameTextList.Count; i++)
        {
            playerNameTextList[i].text = "-";
        }
    }

    private void Ready()
    {
        foreach (var connection in Connection.list)
        {
            connection.currenctCharacter.GetComponent<PlayReadyState>().Ready();
        }
        readyCount++;
        DeactiveByReady();
        RefreshPlayerList().Forget();
    }

    private void DeactiveByReady()
    {
        readyButton.interactable = false;
    }
    
    private void Exit()
    {
        SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();
    }

    public void StateAuthorityChanged()
    {
        Debug.Log($"sachanged");
        RefreshPlayerList().Forget();
        exitButton.onClick.AddListener(Exit);
        readyButton.onClick.AddListener(Ready);
    }
}
