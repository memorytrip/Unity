using UnityEngine;
using Fusion;

public class Photo : NetworkBehaviour
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcDespawn()
    {
        Debug.Log($"Despawn Photo - {Object.HasStateAuthority}");
        
        if (Object.HasStateAuthority)
            Runner.Despawn(Object);
    }
}
