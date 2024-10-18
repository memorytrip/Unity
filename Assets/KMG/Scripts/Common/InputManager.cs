using System;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Common
{
    public class InputManager: MonoBehaviour
    {
        public static InputManager Instance = null;
        
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction lookAction;
        [HideInInspector] public InputAction jumpAction;
    
        public event EventHandler<PinchEventArgs> OnPinch;
        public event EventHandler<PanningEventArgs> OnPanning;
        public event Action<Finger> OnFingerUp;
        public event Action<Finger> OnFingerDown;
        public event Action<Finger> OnFingerMove;

        private float prevPinchMagnitude = -1;
        private Vector2 prevPanningCenter = Vector2.zero;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            
            EnhancedTouchSupport.Enable();
            
            InitAction();
            InitPinch();
            InitPanning();
            
        }

        private void InitAction()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            lookAction = InputSystem.actions.FindAction("Look");
            jumpAction = InputSystem.actions.FindAction("Jump");
            Touch.onFingerUp += finger => OnFingerUp?.Invoke(finger);
            Touch.onFingerMove += finger => OnFingerMove?.Invoke(finger);
            Touch.onFingerDown += finger => OnFingerDown?.Invoke(finger);
        }

        private void InitPinch()
        {
            OnFingerMove += _ =>
            {
                var fingers = Touch.activeFingers;
                if (fingers.Count != 2) return;
                var magnitude = (fingers[0].screenPosition - fingers[1].screenPosition).magnitude;
                if (prevPinchMagnitude < 0f)
                    prevPinchMagnitude = magnitude;
                var difference = magnitude - prevPinchMagnitude;
                prevPinchMagnitude = magnitude;
                OnPinch?.Invoke(this, new PinchEventArgs(difference));
            };
        }

        private void InitPanning()
        {
            OnFingerMove += _ =>
            {
                var fingers = Touch.activeFingers;
                if (fingers.Count != 2) return;
                if (EventSystem.current.IsPointerOverGameObject(0)) return;
                var center = (fingers[0].screenPosition + fingers[1].screenPosition) / 2;
                if (prevPanningCenter == Vector2.zero)
                    prevPanningCenter = center;
                var difference = center - prevPanningCenter;
                prevPanningCenter = center;
                OnPanning?.Invoke(this, new PanningEventArgs(difference));
            };
        }
    }
    
    public class PinchEventArgs : EventArgs
    {
        public float difference;
        public PinchEventArgs(float difference)
        {
            this.difference = difference;
        }
    }

    public class PanningEventArgs : EventArgs
    {
        public Vector2 difference;
        public PanningEventArgs(Vector2 difference)
        {
            this.difference = difference;
        }
    }

}