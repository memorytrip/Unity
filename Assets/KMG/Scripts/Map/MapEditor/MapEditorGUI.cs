using System;
using Common;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

namespace Map.Editor
{
    public class MapEditorGUI : MonoBehaviour
    {
        [SerializeField] public CinemachineInputAxisController cinemachineController;
        public MapEditor target;
        [SerializeField] public Button rotationButton;
        private MapEditorGUIState state;
        
        void Start()
        {
            target = new MapEditor();
            InputManager.Instance.OnFingerDown += OnTouchStart;
            InputManager.Instance.OnFingerMove += OnTouchPerform;
            InputManager.Instance.OnFingerUp += OnTouchCanceled;
            cinemachineController.enabled = false;
            rotationButton.gameObject.SetActive(false);
            state = new MapEditorGUIIdle(this);
        }

        void OnTouchStart(Finger finger)
        {
            state.OnTouchStart(finger);
            SwitchActiveRotationIcon();
        }

        void OnTouchPerform(Finger finger)
        {
            state.OnTouchPerform(finger);
        }

        void OnTouchCanceled(Finger finger)
        {
            state.OnTouchCanceled(finger);
        }

        public void SwitchState(MapEditorGUIState state)
        {
            this.state = state;
        }

        private void SwitchActiveRotationIcon()
        {
            if (target.IsFocusing())
            {
                rotationButton.gameObject.SetActive(true);
                Vector3 buttonPos = Camera.main.WorldToScreenPoint(target.GetPositionOfFocus());
                buttonPos.y += 100;
                rotationButton.transform.position = buttonPos;

            }
            else
            {
                rotationButton.gameObject.SetActive(false);
            }
            
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnFingerDown -= OnTouchStart;
            InputManager.Instance.OnFingerMove -= OnTouchPerform;
            InputManager.Instance.OnFingerUp -= OnTouchCanceled;
        }
    }
}
