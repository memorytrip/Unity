using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Network;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using SceneManager = Common.SceneManager;

public class PhotoManager : NetworkRunnerCallbacks, IListener
{
    private float yValue = 1f;

    private Dictionary<int, Vector3> photoPositions;

    public NetworkPrefabRef photoPrefab;
    private NetworkObject hitNetworkObject;

    private GameObject hitObject;
    private int foundPhoto = 0;
    public TMP_Text findedPhotoCount;
    private int numberOfPhoto;

    void Start()
    {
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
        photoPositions = new Dictionary<int, Vector3>();
    }

    public override void Spawned()
    {
        RpcTotalPhoto();
        Debug.Log(numberOfPhoto);
        HidePhoto(photoPositions, numberOfPhoto).Forget();
    }

    public override void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        findedPhotoCount.text = foundPhoto + " / " + runner.ActivePlayers.Count();
    }

    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EventType.eRaycasting:
                if (param is PlayerInteraction.RaycastInfo raycastInfo)
                {
                    if (raycastInfo.isHit)
                    {
                        hitObject = raycastInfo.hitInfo.collider.gameObject;
                        hitNetworkObject = hitObject.GetComponent<NetworkObject>();
                    }
                }

                break;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcTotalPhoto() //이후 사진 받은걸로 바꿔야함
    {
        numberOfPhoto += 2;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RpcUpdateFoundPhoto()
    {
        foundPhoto++;
        findedPhotoCount.text = foundPhoto + " / " + numberOfPhoto;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcSceneChange()
    {
        SceneManager.Instance.MoveScene("SelectPhotoScene");
    }

    public async UniTaskVoid HidePhoto(Dictionary<int, Vector3> photoDict, int numberOfPhotos)
    {
        for (var i = 1; i <= numberOfPhotos; i++)
        {
            int photoName = i;
            photoDict[photoName] = GetRandomPosition();

                await UniTask.Delay(TimeSpan.FromSeconds(3.5f));
            var photoObject =
                await RunnerManager.Instance.Runner.SpawnAsync(photoPrefab, photoDict[photoName], Quaternion.identity);
            
            var photoData = photoObject.GetComponent<PhotoData>();
            photoData?.SetPhotoName(photoName);
        }

    }

    public void OnDestroyPhoto()
    {
        if (hitNetworkObject != null)
        {
            var photo = hitNetworkObject.GetComponent<Photo>();
            photo.RpcDespawn();
            RpcUpdateFoundPhoto();
            
            if (foundPhoto >= numberOfPhoto)
            {
                RpcSceneChange();
                foundPhoto = 0;
            }
        }
    }

    /*private IEnumerator EndGame()
    {
        findedPhoto = 0;
        yield return SceneManager.Instance.MoveSceneProcess("SelectPhotoScene");
    }*/
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10, 11), yValue, Random.Range(-10, 11));
    }

}

