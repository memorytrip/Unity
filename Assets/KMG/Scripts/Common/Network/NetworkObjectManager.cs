using Fusion;
using UnityEngine;

namespace Common.Network
{
    /**
     * 아바타 생성, NetworkObject 소유권 이전 등을 담당
     */
    public class NetworkObjectManager: SimulationBehaviour, IPlayerJoined, IPlayerLeft
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
    }
}