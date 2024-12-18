using Common.Network;
using Fusion;
using UnityEngine;

public class WebsocketStomp : NetworkBehaviour
{
    public void GetStompData()
    { 
        RpcStompSubscribe();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcStompSubscribe()
    {
        Debug.Log("Stomp Subscribe");
        WebSocketManager.Instance._webSocket.Send("CONNECT\naccept-version:1,2\nhost:125.132.216.190:17778\n\n\0");
        WebSocketManager.Instance._webSocket.Send($"SUBSCRIBE\nid:sub-0\ndestination:/topic/videos/{RunnerManager.Instance.Runner.SessionInfo.Name}\n\n\0");
    }
}
