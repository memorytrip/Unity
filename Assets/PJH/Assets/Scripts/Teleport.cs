using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Myroom;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Button myRoomButton;

    private void Awake()
    {
        myRoomButton.onClick.AddListener(TeleportToMyRoom);
    }
    
    private void TeleportToMyRoom()
    {
        User user = SessionManager.Instance.currentSession?.user;
        if (user != null)
        {
            LoadMyroom.mapOwnerName = user.nickName;
            SceneManager.Instance.MoveRoom($"player_{user.nickName}").Forget();
        }
    }
}
