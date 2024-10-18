// KMG

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// 테스트용 카피본. 사용이 확정될 시 원작성자 코드로 교체 예정
namespace JinsolTest.KMG.Common
{
    public class InputManager: MonoBehaviour
    {
        public static InputManager Instance = null;
        
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction lookAction;
        [HideInInspector] public InputAction touch0Action;
        [HideInInspector] public InputAction touch1Action;
    
        public event EventHandler<PinchEventArgs> OnPinch;
        public event EventHandler<PanningEventArgs> OnPanning;

        [HideInInspector] public int touchCount = 0;

        private float prevPinchMagnitude = -1;
        private Vector2 prevPanningCenter = Vector2.zero;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        
        private void Start()
        {
            InitAction();
            InitPinch();
            InitPanning();
        }

        private void InitAction()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            lookAction = InputSystem.actions.FindAction("Look");
            touch0Action = InputSystem.actions.FindAction("Touch0");
            touch1Action = InputSystem.actions.FindAction("Touch1");

            touch0Action.started += _ => touchCount++;
            touch1Action.started += _ => touchCount++;
            touch0Action.canceled += _ =>
            {
                touchCount--;
                prevPinchMagnitude = -1;
                prevPanningCenter = Vector2.zero;
            };
            touch1Action.canceled += _ =>
            {
                touchCount--;
                prevPinchMagnitude = -1;
                prevPanningCenter = Vector2.zero;
            };
        }

        private void InitPinch()
        {
            touch1Action.performed += _ =>
            {
                if (touchCount != 2) return;
                if (EventSystem.current.IsPointerOverGameObject(0)) return;
                var magnitude = (touch0Action.ReadValue<Vector2>() - touch1Action.ReadValue<Vector2>()).magnitude;
                if (prevPinchMagnitude < 0f)
                    prevPinchMagnitude = magnitude;
                var difference = magnitude - prevPinchMagnitude;
                prevPinchMagnitude = magnitude;
                OnPinch?.Invoke(this, new PinchEventArgs(difference));
            };
        }

        private void InitPanning()
        {
            touch1Action.performed += _ =>
            {
                if (touchCount != 2) return;
                if (EventSystem.current.IsPointerOverGameObject(0)) return;
                var center = (touch0Action.ReadValue<Vector2>() + touch1Action.ReadValue<Vector2>()) / 2;
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