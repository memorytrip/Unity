using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using System;
using System.IO;
using UnityEngine.Serialization;

// ScreenshotWorldCanvas의 Canvas > Event Camera: 메인카메라로 지정 필요
public class ScreenshotManager : MonoBehaviour
{
    // TODO: 스크린샷 기능은 UI에 버튼 형태로 존재할 예정
    // 이지만 몇 가지 제한 사항이 있음
    // 1. 모두가 동시에 관람하는/전환하는 이벤트 발생 시에는 스크린샷 모드가 강제로 해제
    // 2. 세션(방에서 하는 모든 활동 - 영상을 관람하고 각자 저장하는 과정)이 끝난 후 30분간은 자유롭게 맵을 돌아다니면서 촬영 가능
    // 3. 스크린샷 기능은 각자 독립적으로 실행: 서로 상황이 공유되지 않음
    // 3-1. 스크린샷 기능을 활성화 후 카메라 시점(ECameraMode)을 변경할 수 없음
    // 3-2. 스크린샷 기능 활성화 후 포즈를 바꾸거나 맵을 이동할 수 없음
    // 3-3. 단, 스크린샷 이전에 취한 포즈는 그대로 반영된다 (모션을 취하고 적절한 타이밍에 스크린샷 기능을 활성화 하면 좀 더 재밌는 사진을 촬용할 수 있음)
        
    public enum ECameraMode
    {
        Default,
        Screenshot,
    }
        
    public enum ECameraState
    {
        Static,
        Moving,
    }
    
    public enum EScreenshotCameraType
    {
        Default,
        Selfie,
    }

    [Header("메인 UI")] [SerializeField] private Button toggleScreenshotModeButton;
        
    [Header("스크린샷 기능")]
    [SerializeField] private CinemachineCamera[] screenshotCameras = new CinemachineCamera [2];
    private GameObject _screenshotUi;
    [SerializeField] private RawImage cameraPreviewRawImage;
    [SerializeField] private Button flipButton;
    [SerializeField] private Button snapshotButton;
        
    [Header("파일 구조 바뀌면 재연결 필요")]
    [Tooltip(@"Jinsol\Textures\ScreenshotRenderTexture.renderTexture")]
    [SerializeField] private RenderTexture screenshotRenderTexture;

    private ECameraMode _cameraMode;
    private EScreenshotCameraType _screenshotCameraType;
    private bool _isScreenshotModeEnabled;

    private int _layerAsLayerMask;
    private float _maxDistance;
    private float _speed;
    private bool _hitDetected;
    private Collider _hitBoxCollider;
    private RaycastHit _hit;
    
    #region Save

    private const string SubDirectory = "/Screenshots/";
    private const string FileType = ".png";

    #endregion

    private void Awake()
    {
        InitializeDirectory();
        
        _layerAsLayerMask = (1 << LayerMask.NameToLayer("Quest"));
        _maxDistance = 300f;
        _speed = 20f;
        _hitBoxCollider = GetComponent<Collider>();

        var canvas = GetComponentInChildren<Canvas>();
        _screenshotUi = canvas.gameObject;
        if (canvas.worldCamera == null)
        {
            Debug.LogWarning("ScreenshotWorldCanvas -> Canvas -> Event Camera is null: use scene's main camera");
        }
            
        _cameraMode = ECameraMode.Default;
        _screenshotCameraType = EScreenshotCameraType.Default;

        foreach (var screenshotCamera in screenshotCameras)
        {
            screenshotCamera.gameObject.SetActive(false);
        }

        toggleScreenshotModeButton.onClick.AddListener(ToggleScreenshotMode);
        flipButton.onClick.AddListener(FlipCamera);
        snapshotButton.onClick.AddListener(CaptureScreenshot);
            
        _screenshotUi.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_isScreenshotModeEnabled)
        {
            _hitDetected = Physics.BoxCast(_hitBoxCollider.bounds.center, transform.localScale * 0.5f, transform.forward, out _hit, transform.rotation, _maxDistance, _layerAsLayerMask);
            if (_hitDetected)
            {
                Debug.Log("Hit Detected");
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (_hitDetected)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * _hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * _hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * _maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * _maxDistance, transform.localScale);
        }
    }

    private void ToggleScreenshotMode()
    {
        _cameraMode = _cameraMode switch
        {
            ECameraMode.Default => ECameraMode.Screenshot,
            ECameraMode.Screenshot => ECameraMode.Default,
            _ => _cameraMode
        };

        ToggleCamera();
    }

    private void ToggleCamera()
    {
        switch (_cameraMode)
        {
            case ECameraMode.Screenshot:
                screenshotCameras[(int)_screenshotCameraType].gameObject.SetActive(true);
                _isScreenshotModeEnabled = true;
                break;
            case ECameraMode.Default:
            default:
                screenshotCameras[(int)_screenshotCameraType].gameObject.SetActive(false);
                _isScreenshotModeEnabled = false;
                break;
        }

        ToggleScreenshotUI();
    }

    private void ToggleScreenshotUI()
    {
        switch (_cameraMode)
        {
            case ECameraMode.Screenshot:
                _screenshotUi.SetActive(true);
                Debug.Log("Screenshot UI Active");
                break;
            case ECameraMode.Default:
            default:
                _screenshotUi.SetActive(false);
                Debug.Log("Screenshot UI Inactive");
                break;
        }
    }

    private void FlipCamera()
    {
        _screenshotCameraType = _screenshotCameraType switch
        {
            EScreenshotCameraType.Default => EScreenshotCameraType.Selfie,
            EScreenshotCameraType.Selfie => EScreenshotCameraType.Default,
            _ => _screenshotCameraType
        };

        switch (_screenshotCameraType)
        {
            case EScreenshotCameraType.Selfie:
                screenshotCameras[(int)EScreenshotCameraType.Default].gameObject.SetActive(false);
                screenshotCameras[(int)EScreenshotCameraType.Selfie].gameObject.SetActive(true);
                break;
            case EScreenshotCameraType.Default:
            default:
                screenshotCameras[(int)EScreenshotCameraType.Default].gameObject.SetActive(true);
                screenshotCameras[(int)EScreenshotCameraType.Selfie].gameObject.SetActive(false);
                break;
        }
        Debug.Log("Flip Camera!");
    }

    private void InitializeDirectory()
    {
        if (!Directory.Exists(Application.persistentDataPath + SubDirectory))
        {
            Directory.CreateDirectory(Application.persistentDataPath + SubDirectory);
        }
    }
    
    private void CaptureScreenshot()
    {
        if (_hitDetected)
        {
            Debug.Log("Hit Detected");
        }
        
        Debug.Log($"{_layerAsLayerMask}");
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 20000f, _layerAsLayerMask))
        {
            Debug.Log($"Found it! {_layerAsLayerMask}, {hitInfo.colliderInstanceID}");
        }
        else
        {
            Debug.Log($"?, {hitInfo.collider}, {hitInfo.colliderInstanceID}");
        }
        
        var now = DateTime.Now;
        var formattedDate = now.ToString("yyyyMMdd_HHmmssfff");
            
        // Assign the RenderTexture temporarily to the active RenderTexture
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = screenshotRenderTexture;

        // Create a new Texture2D with the RenderTexture's dimensions
        Texture2D screenshot = new Texture2D(screenshotRenderTexture.width, screenshotRenderTexture.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenshotRenderTexture.width, screenshotRenderTexture.height), 0, 0);
        screenshot.Apply();

        // Save the screenshot as a PNG file
        byte[] bytes = screenshot.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath + SubDirectory + formattedDate + FileType);
        File.WriteAllBytes(path, bytes);

        Debug.Log($"Screenshot saved to {path}");

        // Clean up
        RenderTexture.active = currentRT;
        Destroy(screenshot);
    }
}