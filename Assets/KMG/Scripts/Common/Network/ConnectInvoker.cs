using Fusion;
using UnityEngine;

namespace Common.Network
{
    /**
     * Fusion 방 접속/해제시에 일어나야 할 일들 관리 (NetworkObject 소유권 이전 등)
     */
    public class ConnectInvoker: SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        
        
        public void PlayerJoined(PlayerRef player)
        {
            
        }

        public void PlayerLeft(PlayerRef player)
        {
            RequestAuth();
        }

        /**
         * 오브젝트의 소유권이 비어있으면 취득
         */
        public void RequestAuth()
        {
            foreach (NetworkObject obj in Runner.GetAllNetworkObjects())
            {
                if (obj.StateAuthority == PlayerRef.None)
                {
                    obj.RequestStateAuthority();
                    Debug.Log($"Transferred ownership of [{obj.name}] to Player [{Runner.LocalPlayer}]");
                }
            }
        }

        /**
         * 오브젝트의 소유권이 자신이면 소유권을 해제
         */
        public void ReleaseAuth(PlayerRef playerSelf)
        {
            foreach (NetworkObject obj in Runner.GetAllNetworkObjects())
            {
                if (obj.StateAuthority == playerSelf)
                {
                    obj.ReleaseStateAuthority();
                    Debug.Log($"Released ownership of [{obj.name}] from Player [{playerSelf}]");
                }
            }
        }
        
        // public void SpawnAvatar
    }
}