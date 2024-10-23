using System;
using System.Collections;
using Common;
using UnityEngine;
using GUI;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CinemachineInput : MonoBehaviour
{
    private CinemachineInputAxisController ax;
    private CinemachineOrbitalFollow orbitalFollow;
    private int lookFingerIndex = -1;
    private Vector2 previousScreenPosition;

    private Coroutine retarget;
    private WaitForSeconds wfs = new WaitForSeconds(1f);

    void Awake()
    {
        // ax = GetComponent<CinemachineInputAxisController>();
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    /**
     * 터치 시작 시 GUI 바깥인지 확인 후 해당 터치 인덱스를 저장
     * 터치 중에 delta값을 구해 카메라 회전
     * 터치가 끝나면 RetargetProcess 실행
     */
    private void Start()
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
        orbitalFollow.HorizontalAxis.Recentering.Enabled = false;
        orbitalFollow.VerticalAxis.Recentering.Enabled = false;
    }

    void OnTouchPerform(Finger finger)
    {
        if (finger.index != lookFingerIndex)
            return;
        Vector2 delta = (finger.screenPosition - previousScreenPosition) / 10f;
        orbitalFollow.HorizontalAxis.Value += delta.x;
        orbitalFollow.VerticalAxis.Value -= delta.y;
        orbitalFollow.VerticalAxis.Value = Mathf.Clamp(orbitalFollow.VerticalAxis.Value, -10, 45);
        previousScreenPosition = finger.screenPosition;
    }

    void OnTouchEnd(Finger finger)
    {
        if (finger.index == lookFingerIndex)
        {
            lookFingerIndex = -1;
            previousScreenPosition = Vector2.zero;
        }

        if (Touch.activeFingers.Count == 1)
        {
            retarget = StartCoroutine(RetargetProcess());
        }
    }

    IEnumerator RetargetProcess()
    {
        yield return wfs;
        orbitalFollow.HorizontalAxis.Recentering.Enabled = true;
        orbitalFollow.VerticalAxis.Recentering.Enabled = true;
    }
}