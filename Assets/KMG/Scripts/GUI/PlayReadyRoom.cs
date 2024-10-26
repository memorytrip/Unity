using System.Collections.Generic;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayReadyRoom : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private List<TMP_Text> playerNameList;
    [SerializeField] private Button exitButton;
    
    public override void Spawned()
    {
        Connection.StateAuthInstance.OnAfterSpawned += RefreshPlayerList;
        exitButton.onClick.AddListener(Exit);
    }
    
    public void PlayerJoined(PlayerRef player)
    {
        RefreshPlayerList();
    }

    public void PlayerLeft(PlayerRef player)
    {
        RefreshPlayerList();
    }

    private void RefreshPlayerList()
    {
        var i = 0;
        foreach (var connection in Connection.list)
        {
            playerNameList[i++].text = $"[{(connection.hasSceneAuthority ? 'O' : 'X')}] {connection.playerName}";
        }

        for (; i < playerNameList.Count; i++)
        {
            playerNameList[i].text = "-";
        }
    }

    private void Ready()
    {
        RpcReady(Connection.StateAuthInstance);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcReady(Connection connection)
    {
        Debug.Log($"{connection.playerName} has ready");
    }

    private void Exit()
    {
        SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();
    }
}
