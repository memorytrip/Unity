using Fusion;
using UnityEngine;

namespace GUI
{
    public class PlayReadyState: NetworkBehaviour 
    {
        [Networked] public bool ready { get; set; }


		public void Ready()
        {
            if (HasStateAuthority) 
                RpcReady();
        }

        // [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RpcReady()
        {
            // ready = true;
            ready = !ready;
        }
    }
}