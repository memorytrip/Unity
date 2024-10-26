using Fusion;
using UnityEngine;

namespace GUI
{
    public class PlayReadyState: NetworkBehaviour
    {
        public bool ready = false;

        
        public void Ready()
        {
            if (HasStateAuthority) 
                RpcReady();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcReady()
        {
            ready = !ready;
        }
    }
}