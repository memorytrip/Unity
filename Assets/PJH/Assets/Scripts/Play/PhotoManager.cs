using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Network;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using Fusion;
using GUI;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using SceneManager = Common.SceneManager;

public class PhotoManager : NetworkRunnerCallbacks, IListener
{
    private float yValue = 15f;
    private Dictionary<int, Vector3> photoPositions;
    public NetworkPrefabRef photoPrefab;
    private GameObject hitObject;
    private int findedPhoto = 0;
    private NetworkObject hitNetworkObject;
    public TMP_Text findedPhotoCount;
    private int numberOfPhoto;
    private readonly WaitForSeconds _waitForAnimation = new WaitForSeconds(3f);
    [SerializeField] private LoadFindPhotoMap loadFindPhotoMap;
    [SerializeField] private GameObject photoGround;
    [SerializeField] private Button skipButton;
    [SerializeField] private GameObject creditParticle;
    // [SerializeField] private CanvasGroup helpPopup;
    [SerializeField] private FadeController helpPopup;

    private const string PhotoCountText = "내가 찾은 사진: ";

    void Start()
    {
        EventManager.Instance.LoadCompleteMap += ShowScreen;
        StartCoroutine(helpPopup.FadeIn(0f));
        EventManager.Instance.AddListener(EventType.eRaycasting, this);
        photoPositions = new Dictionary<int, Vector3>();
        skipButton.onClick.AddListener(() =>
            RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.LetterFoundCount, 8));
        skipButton.onClick.AddListener(() => StartCoroutine(FindLastPhoto()) );
    }
    
    public override void Spawned()
    {
        RpcTotalPhoto();
        Debug.Log(numberOfPhoto);
        HidePhoto(photoPositions, numberOfPhoto).Forget();
        skipButton.gameObject.SetActive(true);
        skipButton.interactable = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcTotalPhoto() //이후 사진 받은걸로 바꿔야함
    {
        numberOfPhoto += 2;
    }
    
    private async UniTask<Vector3> GetRandomPosition()
    {
        Vector3 rayOrigin = new Vector3(Random.Range(-40, 40), yValue, Random.Range(-40, 40));
        RaycastHit hit;
        bool ishit;
        int testCount = 0;
        do
        {
            rayOrigin = new Vector3(Random.Range(-40, 40), yValue, Random.Range(-40, 40));
            ishit = Physics.Raycast(rayOrigin, Vector3.down, out hit, yValue);
            await UniTask.Yield();
            if (testCount++ > 5000)
            {
                Debug.LogWarning("GetRandomPosition: Failed to posit photo");
                break;
            }
                
        } while (ishit == false);
        return hit.point;
    }

    public async UniTaskVoid HidePhoto(Dictionary<int, Vector3> photoDict, int numberOfPhotos)
    {
        await UniTask.WaitWhile(() => loadFindPhotoMap.isLoading);
        for (var i = 1; i <= numberOfPhotos; i++)
        {
            int photoName = i;
            photoDict[photoName] = await GetRandomPosition();
            // await UniTask.Delay(TimeSpan.FromSeconds(3.5f));
            var photoObject =
                await RunnerManager.Instance.Runner.SpawnAsync(photoPrefab, photoDict[photoName], Quaternion.identity);
            
            var photoData = photoObject.GetComponent<PhotoData>();
            photoData?.SetPhotoName(photoName);
        }
        Destroy(photoGround);
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

    public void OnDestroyPhoto()
    {
        if (hitNetworkObject != null)
        {
            RuntimeManager.StudioSystem.setParameterByName(ParameterNameCache.LetterFoundCount, findedPhoto);
            var photo = hitNetworkObject.GetComponent<Photo>();
            photo.RpcDespawn();
            RpcUpdateFindedPhoto();
            Instantiate(creditParticle, hitNetworkObject.transform.position, Quaternion.identity);
            SessionManager.Instance.currentUser.credit++;
            if (findedPhoto >= numberOfPhoto)
            {
                StartCoroutine(FindLastPhoto());
            }
        }
    }

    private IEnumerator FindLastPhoto()
    {
        yield return _waitForAnimation;
        RpcSceneChange();
        findedPhoto = 0;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcUpdateFindedPhoto()
    {
        findedPhoto++;
        EventManager.Instance.OnPhotoFound(findedPhoto);
        findedPhotoCount.text = PhotoCountText + findedPhoto + " / " + numberOfPhoto;
    }
    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcSceneChange()
    {
        SceneManager.Instance.MoveScene("SelectPhotoScene");
    }

    /*private IEnumerator EndGame()
    {
        findedPhoto = 0;
        yield return SceneManager.Instance.MoveSceneProcess("SelectPhotoScene");
    }*/
    public override void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        findedPhotoCount.text = PhotoCountText + findedPhoto + " / " + runner.ActivePlayers.Count();
    }

    private void ShowScreen()
    {
        // helpPopup.alpha = 0f;
        StartCoroutine(helpPopup.FadeOut(1f));
        EventManager.Instance.LoadCompleteMap -= ShowScreen;
    }
}