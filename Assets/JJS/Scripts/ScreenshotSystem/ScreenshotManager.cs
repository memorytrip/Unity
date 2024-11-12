using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using System;
using System.IO;
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

    [Header("메인 UI")] [SerializeField] private Button toggleScreenshotModeButton;
    
    private ScreenshotCamera[] _targetCameras = new ScreenshotCamera[2]; 
    [SerializeField] private CinemachineCamera[] _screenshotCameras = new CinemachineCamera[2];
    [SerializeField] private Camera[] _screenshotCamerasCamComponent = new Camera[2];
    [SerializeField] private CanvasGroup screenshotUi;
    [SerializeField] private RawImage cameraPreviewRawImage;
    [SerializeField] private Button flipButton;
    [SerializeField] private Button snapshotButton;
    [SerializeField] private Button exitButton;

    [Header("파일 구조 바뀌면 재연결 필요")] [Tooltip(@"Jinsol\Textures\ScreenshotRenderTexture.renderTexture")] [SerializeField]
    private RenderTexture screenshotRenderTexture;

    public static ECameraMode CameraMode { get; private set; }
    public static EScreenshotCameraType ScreenshotCameraType;
    public static bool IsScreenshotModeEnabled;

    private Slider _slider;
    [SerializeField] private float zoomRate;
    private const float MinFieldOfView = 5f;
    private const float MaxFieldOfView = 90f;

    // Quest
    private Vector3 _rayDirection;
    [SerializeField] private GameObject questAlert;
    private bool snapshotQuestCleared;

    // QR
    private int _qrLayerMask;
    [SerializeField] private CanvasGroup jumpToInternetPopup;
    [SerializeField] private Button jumpToInternetButton;
    private string _savedURL;

    private float DefaultFOV()
    {
        return _screenshotCameras[(int)EScreenshotCameraType.Selfie].IsLive ? 24f : 36f;
    }

    #region Save System

    private string _directory;
    private const string SubDirectory = "/Screenshots/";
    private const string FileType = ".png";

    #endregion

    private void Start()
    {
        toggleScreenshotModeButton = FindAnyObjectByType<ScreenshotButton>().GetComponent<Button>();
        InitializeScreenshotFeature();
        InitializeCameras();
    }

    private void InitializeScreenshotFeature()
    {
        InitializeDirectory();

        HideUi(screenshotUi);

        _slider = GetComponentInChildren<Slider>();
        _slider.minValue = MinFieldOfView;
        _slider.maxValue = MaxFieldOfView;

        var canvas = GetComponentInChildren<Canvas>();
        if (canvas.worldCamera == null)
        {
            Debug.LogWarning("ScreenshotWorldCanvas -> Canvas -> Event Camera is null: use scene's main camera");
        }

        toggleScreenshotModeButton.onClick.AddListener(ToggleScreenshotMode);
        flipButton.onClick.AddListener(FlipCamera);
        snapshotButton.onClick.AddListener(CaptureScreenshot);
        jumpToInternetButton.onClick.AddListener(JumpToURL);
        exitButton.onClick.AddListener(ToggleScreenshotMode);

        _slider.onValueChanged.AddListener(ControlCameraZoom);
    }

    private void InitializeCameras()
    {
        //_targetCameras =
        
        CameraMode = ECameraMode.Default;
        ScreenshotCameraType = EScreenshotCameraType.Default;

        foreach (var screenshotCamera in _screenshotCameras)
        {
            screenshotCamera.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        DetectQuestObject();
    }

    private void DetectQuestObject()
    {
        questAlert.SetActive(RaycastShooter.HitDetected);
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
                IsScreenshotModeEnabled = true;
                break;
            case ECameraMode.Default:
                IsScreenshotModeEnabled = false;
                foreach (var ssCamera in _screenshotCameras)
                {
                    ssCamera.gameObject.SetActive(false);
                    Debug.Log($"SSCamera : {ssCamera.gameObject} set to false? {ssCamera.isActiveAndEnabled}");
                }
                break;
        }

        ToggleScreenshotUI();
    }

    private void ToggleScreenshotUI()
    {
        ResetCameraMode();
        ResetCameraZoom();

        switch (CameraMode)
        {
            case ECameraMode.Screenshot:
                ShowUi(screenshotUi);
                Debug.Log("Screenshot UI Active");
                break;
            case ECameraMode.Default:
            default:
                HideUi(screenshotUi);
                Debug.Log("Screenshot UI Inactive");
                break;
        }
    }

    private void ShowUi(CanvasGroup target)
    {
        target.interactable = true;
        target.blocksRaycasts = true;
        target.alpha = 1f;
    }

    private void HideUi(CanvasGroup target)
    {
        target.interactable = false;
        target.blocksRaycasts = false;
        target.alpha = 0f;
    }

    private void FlipCamera()
    {
        ScreenshotCameraType = ScreenshotCameraType switch
        {
            EScreenshotCameraType.Default => EScreenshotCameraType.Selfie,
            EScreenshotCameraType.Selfie => EScreenshotCameraType.Default,
            _ => ScreenshotCameraType
        };

        switch (ScreenshotCameraType)
        {
            case EScreenshotCameraType.Selfie:
                _screenshotCameras[(int)EScreenshotCameraType.Default].gameObject.SetActive(false);
                _screenshotCameras[(int)EScreenshotCameraType.Selfie].gameObject.SetActive(true);
                break;
            case EScreenshotCameraType.Default:
            default:
                _screenshotCameras[(int)EScreenshotCameraType.Default].gameObject.SetActive(true);
                _screenshotCameras[(int)EScreenshotCameraType.Selfie].gameObject.SetActive(false);
                break;
        }

        ResetCameraZoom();
        Debug.Log("Flip Camera!");
    }

    private void ControlCameraZoom(float value)
    {
        if (!IsScreenshotModeEnabled)
        {
            return;
        }

        foreach (var screenshotCamera in _screenshotCamerasCamComponent)
        {
            screenshotCamera.fieldOfView = _slider.value;
        }
    }

    private void ResetCameraZoom()
    {
        foreach (var screenshotCamera in _screenshotCamerasCamComponent)
        {
            screenshotCamera.fieldOfView = DefaultFOV();
            _slider.value = DefaultFOV();
            Debug.Log($"Field of View reset to {DefaultFOV()}");
        }
    }

    private void ResetCameraMode()
    {
        if (IsScreenshotModeEnabled)
        {
            ScreenshotCameraType = EScreenshotCameraType.Default;
            _screenshotCameras[(int)EScreenshotCameraType.Default].gameObject.SetActive(true);
            _screenshotCameras[(int)EScreenshotCameraType.Selfie].gameObject.SetActive(false);
        }
    }

    private void InitializeDirectory()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            _directory = Application.persistentDataPath;
            
            if (!Directory.Exists(_directory + SubDirectory))
            {
                Directory.CreateDirectory(_directory + SubDirectory);
            }
        }
        else
        {
            _directory = GetAndroidExternalStoragePath();
            
            if (!Directory.Exists(_directory + SubDirectory))
            {
                Directory.CreateDirectory(_directory + SubDirectory);
            }
        }
    }

    private string GetAndroidExternalStoragePath()
    {
        var androidJavaClass = new AndroidJavaClass("android.os.Environment");
        var path = androidJavaClass.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", androidJavaClass.GetStatic<string>("DIRECTORY_DCIM")).Call<string>("getAbsolutePath");
        return path;
    }

    private void CaptureScreenshot()
    {
        // Assign the RenderTexture temporarily to the active RenderTexture
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = screenshotRenderTexture;

        // Create a new Texture2D with the RenderTexture's dimensions
        Texture2D screenshot = new Texture2D(screenshotRenderTexture.width, screenshotRenderTexture.height,
            TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenshotRenderTexture.width, screenshotRenderTexture.height), 0, 0);
        screenshot.Apply();

        if (RaycastShooter.HitDetected)
        {
            if (snapshotQuestCleared)
            {
                return;
            }
            YamiQuestManager.Instance.ProceedQuest();
            snapshotQuestCleared = true;
        }

        if (RaycastShooter.FoundQRCode)
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
        string path = Path.Combine(_directory + SubDirectory + formattedDate + FileType);
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
        ShowUi(jumpToInternetPopup);
    }

    private void JumpToURL()
    {
        Application.OpenURL(_savedURL);
        HideUi(jumpToInternetPopup);
    }
}