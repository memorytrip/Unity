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
        [SerializeField] public GameObject rotationButton;
        [SerializeField] public Button undoButton;
        private MapEditorGUIState state;
        
        void Start()
        {
            target = new MapEditor();
            InputManager.Instance.OnFingerDown += OnTouchStart;
            InputManager.Instance.OnFingerMove += OnTouchPerform;
            InputManager.Instance.OnFingerUp += OnTouchCanceled;
            cinemachineController.enabled = false;
            rotationButton.gameObject.SetActive(false);
            undoButton.onClick.AddListener(target.Undo);
            state = new MapEditorGUIIdle(this);
        }

        private void Update()
        {
            UpdateRotationIcon();
        }

        void OnTouchStart(Finger finger)
        {
            state.OnTouchStart(finger);
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
        

        private void UpdateRotationIcon()
        {
            if (target.IsFocusing() && target.GetActiveOfFocus())
            {
                rotationButton.gameObject.SetActive(true);
                Vector3 buttonPos = Camera.main.WorldToScreenPoint(target.GetPositionOfFocus());
                buttonPos.y += 100;
                rotationButton.transform.position = buttonPos;
            } else
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
        

        public static Vector3 QuantizatePosition(Vector3 pos)
        {
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y) + 0.5f;
            pos.z = Mathf.Round(pos.z);
            return pos;
        }
    }
}
