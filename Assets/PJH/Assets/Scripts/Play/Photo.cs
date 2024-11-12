using UnityEngine;
using Fusion;

public class Photo : NetworkBehaviour
{
    public PhotoLetterResponse Info { get; set; }
    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcDespawn()
    {
        if (Object.HasStateAuthority)
            Runner.Despawn(Object);
    }
}
