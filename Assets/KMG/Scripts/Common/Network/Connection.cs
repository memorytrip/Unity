using Fusion;
using UnityEngine;

namespace Common.Network
{
    /**
     * Photon 접속자마다 생성되어 각 접속자 정보, 아바타 관리하는 오브젝트
     * DontDestroyOnLoad
     */
    public class Connection: NetworkBehaviour
    {
        public static Connection StateAuthInstance;

        public NetworkObject currenctCharacter;
        public string playerName;

        public override void Spawned()
        {
            if (!HasStateAuthority) return;
            StateAuthInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}