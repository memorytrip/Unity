using Common;
using UnityEngine;
using GUI;
using Unity.Cinemachine;
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

    private void OnDisable()
    {
        InputManager.Instance.OnFingerDown -= OnTouchStart;
        InputManager.Instance.OnFingerMove -= OnTouchPerform;
        InputManager.Instance.OnFingerUp -= OnTouchEnd;
    }
}