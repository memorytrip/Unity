using System;
using Common;
using UnityEngine;
using GUI;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class CinemachineInput : MonoBehaviour
{
    private CinemachineInputAxisController ax;
    private CinemachineOrbitalFollow orbitalFollow;
    private int lookFingerIndex = -1;
    private Vector2 previousScreenPosition;

    void Awake()
    {
        // ax = GetComponent<CinemachineInputAxisController>();
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    private void Start()
    {
        InputManager.Instance.OnFingerDown += OnTouchStart;
        InputManager.Instance.OnFingerMove += OnTouchPerform;
        InputManager.Instance.OnFingerUp += OnTouchEnd;
    }

    // void Update()
    // {
    //     
    //     if (Utility.IsPointOverGUI())
    //     {
    //         ax.enabled = false;
    //     }
    //     else
    //     {
    //         ax.enabled = true;
    //     }
    // }

    void OnTouchStart(Finger finger)
    {
        if (!Utility.IsPointOverGUI(finger.screenPosition))
        {
            lookFingerIndex = finger.index;
            previousScreenPosition = finger.screenPosition;
        }
    }

    void OnTouchPerform(Finger finger)
    {
        if (finger.index != lookFingerIndex)
            return;
        Vector2 delta = (finger.screenPosition - previousScreenPosition) / 10f;
        orbitalFollow.HorizontalAxis.Value += delta.x;
        orbitalFollow.VerticalAxis.Value -= delta.y;
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
}