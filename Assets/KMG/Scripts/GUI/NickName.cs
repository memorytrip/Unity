using Common.Network;
using Fusion;
using TMPro;
using UnityEngine;

namespace GUI
{
    public class NickName: NetworkBehaviour
    {
        [SerializeField] private TMP_Text nicknameText;
        public override void Spawned()
        {
            NetworkObject networkObject = GetComponentInParent<NetworkObject>();
            Connection connection = Connection.list.Find(
                (c) => c.playerRef == networkObject.StateAuthority
                );
            nicknameText.text = connection.playerName;
        }
    }
}