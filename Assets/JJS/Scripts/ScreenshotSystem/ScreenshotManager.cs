using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using System;
using System.IO;
using UnityEngine.Serialization;
using ZXing;

// ScreenshotWorldCanvas의 Canvas > Event Camera: 메인카메라로 지정 필요
public class ScreenshotManager : MonoBehaviour
{
    // 스크린샷 기능은 UI에 버튼 형태로 존재할 예정
    // 이지만 몇 가지 제한 사항이 있음
    // 1. 모두가 동시에 관람하는/전환하는 이벤트 발생 시에는 스크린샷 모드가 강제로 해제
    // 2. 세션(방에서 하는 모든 활동 - 영상을 관람하고 각자 저장하는 과정)이 끝난 후 30분간은 자유롭게 맵을 돌아다니면서 촬영 가능
    // 3. 스크린샷 기능은 각자 독립적으로 실행: 서로 상황이 공유되지 않음
    // 3-1. 스크린샷 기능을 활성화 후 카메라 시점(ECameraMode)을 변경할 수 없음
    // 3-2. 스크린샷 기능 활성화 후 포즈를 바꾸거나 맵을 이동할 수 없음
    // 3-3. 단, 스크린샷 이전에 취한 포즈는 그대로 반영된다 (모션을 취하고 적절한 타이밍에 스크린샷 기능을 활성화 하면 좀 더 재밌는 사진을 촬용할 수 있음)
        
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

    [SerializeField] private Camera[] screenshotCamerasCamComponent = new Camera[2];
    [SerializeField] private CanvasGroup screenshotUi;
    [SerializeField] private RawImage cameraPreviewRawImage;
    [SerializeField] private Button flipButton;
    [SerializeField] private Button snapshotButton;
    [SerializeField] private Button exitButton;    
    
    [Header("파일 구조 바뀌면 재연결 필요")]
    [Tooltip(@"Jinsol\Textures\ScreenshotRenderTexture.renderTexture")]
    [SerializeField] private RenderTexture screenshotRenderTexture;

    public static ECameraMode CameraMode { get; private set; }
    private EScreenshotCameraType _screenshotCameraType;
    private bool _isScreenshotModeEnabled;
    
    private Slider _slider;
    [SerializeField] private float zoomRate;
    private const float MinFieldOfView = 5f;
    private const float MaxFieldOfView = 90f;

    // Quest
    private Vector3 _rayDirection;
    [SerializeField] private GameObject questAlert;
    
    // QR
    private int _qrLayerMask;
    [SerializeField] private CanvasGroup jumpToInternetPopup;
    [SerializeField] private Button jumpToInternetButton;
    private string _savedURL;
    
    private float DefaultFOV()
    {
        return screenshotCameras[(int)EScreenshotCameraType.Selfie].IsLive ? 24f : 36f;
    }

    // 미션용 세팅: 특명! 오브젝트의 사진을 찍어라!
    private int _layerAsLayerMask;
    private float _maxDistance;
    private float _speed;
    private bool _hitDetected;
    private Collider _hitBoxCollider;
    private RaycastHit _hit;
    
    #region Save System

    private const string SubDirectory = "/Screenshots/";
    private const string FileType = ".png";

    #endregion

    private void Awake()
    {
        InitializeDirectory();

        screenshotUi.alpha = 0f;
        
        // 오브젝트 특정을 위한 부분 초기화
        _layerAsLayerMask = 1 << LayerMask.NameToLayer("Quest");
        _qrLayerMask = 1 << LayerMask.NameToLayer("QR");
        _maxDistance = 300f;
        _speed = 20f;
        _hitBoxCollider = GetComponent<Collider>();

        _slider = GetComponentInChildren<Slider>();
        _slider.minValue = MinFieldOfView;
        _slider.maxValue = MaxFieldOfView;
        
        var canvas = GetComponentInChildren<Canvas>();
        if (canvas.worldCamera == null)
        {
            Debug.LogWarning("ScreenshotWorldCanvas -> Canvas -> Event Camera is null: use scene's main camera");
        }
            
        CameraMode = ECameraMode.Default;
        _screenshotCameraType = EScreenshotCameraType.Default;

        foreach (var screenshotCamera in screenshotCameras)
        {
            screenshotCamera.gameObject.SetActive(false);
        }

        toggleScreenshotModeButton.onClick.AddListener(ToggleScreenshotMode);
        flipButton.onClick.AddListener(FlipCamera);
        snapshotButton.onClick.AddListener(CaptureScreenshot);
        jumpToInternetButton.onClick.AddListener(JumpToURL);
        exitButton.onClick.AddListener(ToggleScreenshotMode);
        
        _slider.onValueChanged.AddListener(ControlCameraZoom);
    }

    private void FixedUpdate()
    {
        SwitchRayDirection();
        DetectQuestObject();
    }

    private void SwitchRayDirection()
    {
        switch (_screenshotCameraType)
        {
            case EScreenshotCameraType.Selfie:
                _rayDirection = -transform.forward;
                break;
            case EScreenshotCameraType.Default:
            default:
                _rayDirection = transform.forward;
                break;
        }
    }
    
    private void DetectQuestObject()
    {
        if (!_isScreenshotModeEnabled)
        {
            return;
        }
        
        _hitDetected = Physics.BoxCast(_hitBoxCollider.bounds.center, transform.localScale * 0.5f, _rayDirection, out _hit, transform.rotation, _maxDistance, _layerAsLayerMask);
        questAlert.SetActive(_hitDetected);
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
        CameraMode = CameraMode switch
        {
            ECameraMode.Default => ECameraMode.Screenshot,
            ECameraMode.Screenshot => ECameraMode.Default,
            _ => CameraMode
        };

        ToggleCamera();
    }

    private void ToggleCamera()
    {
        switch (CameraMode)
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
        ResetCameraZoom();
        ResetCameraMode();

        switch (CameraMode)
        {
            case ECameraMode.Screenshot:
                screenshotUi.alpha = 1f;
                Debug.Log("Screenshot UI Active");
                break;
            case ECameraMode.Default:
            default:
                screenshotUi.alpha = 0f;
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

        ResetCameraZoom();
        Debug.Log("Flip Camera!");
    }

    private void ControlCameraZoom(float value)
    {
        if (!_isScreenshotModeEnabled)
        {
            return;
        }
        
        foreach (var screenshotCamera in screenshotCamerasCamComponent)
        {
            screenshotCamera.fieldOfView = _slider.value;
        }
    }

    private void ResetCameraZoom()
    {
        foreach (var screenshotCamera in screenshotCamerasCamComponent)
        {
            screenshotCamera.fieldOfView = DefaultFOV();
            _slider.value = DefaultFOV();
            Debug.Log($"Field of View reset to {DefaultFOV()}");
        }
    }

    private void ResetCameraMode()
    {
        _screenshotCameraType = EScreenshotCameraType.Default;
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
        // Assign the RenderTexture temporarily to the active RenderTexture
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = screenshotRenderTexture;

        // Create a new Texture2D with the RenderTexture's dimensions
        Texture2D screenshot = new Texture2D(screenshotRenderTexture.width, screenshotRenderTexture.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenshotRenderTexture.width, screenshotRenderTexture.height), 0, 0);
        screenshot.Apply();
        
        if (Physics.Raycast(transform.position, _rayDirection, out RaycastHit hitInfo, 20000f, _layerAsLayerMask))
        {
            YamiQuestManager.Instance.ProceedQuest();
            NullifyQuest(hitInfo);
        }

        if (Physics.Raycast(transform.position, _rayDirection, out hitInfo, 20000f, _qrLayerMask))
        {
            DecodeQRCode(screenshot);
        }
        
        SaveScreenshot(screenshot);
        
        // Clean up
        RenderTexture.active = currentRT;
        Destroy(screenshot);
    }

    private void SaveScreenshot(Texture2D screenshot)
    {
        // Save the screenshot as a PNG file
        var now = DateTime.Now;
        var formattedDate = now.ToString("yyyyMMdd_HHmmssfff");
            
        byte[] bytes = screenshot.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath + SubDirectory + formattedDate + FileType);
        File.WriteAllBytes(path, bytes);

        Debug.Log($"Screenshot saved to {path}");
    }

    private void DecodeQRCode(Texture2D screenshot)
    {
        var barcodeReader = new BarcodeReader();
        Result result = barcodeReader.Decode(screenshot.GetPixels32(), screenshot.width, screenshot.height);
        if (result == null)
        {
            return;
        }
        
        Debug.Log($"QR code found!: BarcodeFormat({result.BarcodeFormat}), ResultText({result.Text})");
        _savedURL = result.Text;
        jumpToInternetPopup.alpha = 1f;
    }

    private void JumpToURL()
    {
        Application.OpenURL(_savedURL);
        jumpToInternetPopup.alpha = 0f;
    }

    private void NullifyQuest(RaycastHit hitInfo)
    {
        hitInfo.collider.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}