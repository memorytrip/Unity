using Common;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUI : MonoBehaviour
    {
        [SerializeField] public CinemachineInputAxisController cinemachineController;
        public MapEditor target;
        private MapEditorGUIState state;
        
        void Start()
        {
            target = new MapEditor();
            InputManager.Instance.OnFingerDown += OnTouchStart;
            InputManager.Instance.OnFingerMove += OnTouchPerform;
            InputManager.Instance.OnFingerUp += OnTouchCanceled;
            cinemachineController.enabled = false;
            state = new MapEditorGUIIdle(this);
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
    }
}
