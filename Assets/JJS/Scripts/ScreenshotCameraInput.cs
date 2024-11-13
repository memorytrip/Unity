using Common;
using UnityEngine;
using GUI;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class ScreenshotCameraInput : MonoBehaviour
{
    private CinemachineInputAxisController ax;
    private CinemachinePanTilt _cinemachinePanTilt;
    private int lookFingerIndex = -1;
    private Vector2 previousScreenPosition;

    private Coroutine retarget;

    void Awake()
    {
        _cinemachinePanTilt = GetComponent<CinemachinePanTilt>();
    }

    /**
     * 터치 시작 시 GUI 바깥인지 확인 후 해당 터치 인덱스를 저장
     * 터치 중에 delta값을 구해 카메라 회전
     */
    private void OnEnable()
    {
        InputManager.Instance.OnFingerDown += OnTouchStart;
        InputManager.Instance.OnFingerMove += OnTouchPerform;
        InputManager.Instance.OnFingerUp += OnTouchEnd;
    }

    private void FixedUpdate()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
        if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
        {
            HandleMouseControl();
        }
#endif
    }

    void OnTouchStart(Finger finger)
    {
        if (!Utility.IsPointOverGUI(finger.screenPosition))
        {
            lookFingerIndex = finger.index;
            previousScreenPosition = finger.screenPosition;
        }

        if (retarget != null)
            StopCoroutine(retarget);
    }

    void OnTouchPerform(Finger finger)
    {
        if (finger.index != lookFingerIndex)
            return;
        Vector2 delta = (finger.screenPosition - previousScreenPosition) / 10f;
        _cinemachinePanTilt.PanAxis.Value += delta.x;
        _cinemachinePanTilt.TiltAxis.Value -= delta.y;
        _cinemachinePanTilt.TiltAxis.Value = Mathf.Clamp(_cinemachinePanTilt.TiltAxis.Value, -10, 45);
        previousScreenPosition = finger.screenPosition;
    }

    void OnTouchEnd(Finger finger)
    {
        if (finger.index == lookFingerIndex)
        {
            lookFingerIndex = -1;
            previousScreenPosition = Vector2.zero;
        }
    }
    
    private void HandleMouseControl()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() / 10f;

        // Disable recentering while controlling with mouse
        _cinemachinePanTilt.PanAxis.Recentering.Enabled = false;
        _cinemachinePanTilt.TiltAxis.Recentering.Enabled = false;

        // Apply mouse delta to camera rotation
        _cinemachinePanTilt.PanAxis.Value += mouseDelta.x;
        _cinemachinePanTilt.TiltAxis.Value -= mouseDelta.y;

        // Clamp the vertical axis as in the touch control
        _cinemachinePanTilt.TiltAxis.Value = Mathf.Clamp(_cinemachinePanTilt.TiltAxis.Value, -10, 45);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnFingerDown -= OnTouchStart;
        InputManager.Instance.OnFingerMove -= OnTouchPerform;
        InputManager.Instance.OnFingerUp -= OnTouchEnd;
    }
}