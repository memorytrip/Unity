using System;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MapListDefaultItem : MonoBehaviour
{
    [HideInInspector] public string mapId;
    [SerializeField] private Image thumbnail;
    [SerializeField] private Button editButton;
    [SerializeField] private Button setHomeButton;

    private void Awake()
    {
        editButton.onClick.AddListener(Edit);
        setHomeButton.onClick.AddListener(SetHome);
    }

    public void SetThumbnail(Sprite sprite)
    {
        thumbnail.sprite = sprite;
    }

    private void Edit()
    {
        EnterEditMode().Forget();
    }

    private async UniTaskVoid EnterEditMode()
    {
        // TODO: 맵에디터로 mapInfo 넘겨주기
        await RunnerManager.Instance.Disconnect();
        await SceneManager.Instance.MoveSceneProcess("MapEdit");
    }

    private void SetHome()
    {
        // TODO: 마이룸 설정 과정
        string playerId = SessionManager.Instance.currentUser.nickName;
        SceneManager.Instance.MoveRoom($"player_{playerId}").Forget();
    }
}
