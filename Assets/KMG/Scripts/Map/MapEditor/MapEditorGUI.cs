using Common;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUI : MonoBehaviour
    {
        [SerializeField] private CinemachineInputAxisController cinemachineController;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InputManager.Instance.OnFingerDown += OnTouchStart;
            InputManager.Instance.OnFingerMove += OnTouchPerform;
            InputManager.Instance.OnFingerUp += OnTouchCanceled;
            cinemachineController.enabled = false;
        }

        void OnTouchStart(Finger finger)
        {
            if (GUI.Utility.IsPointOverGUI(finger.screenPosition))
            {
                cinemachineController.enabled = false;
            }
            else
            {
                cinemachineController.enabled = true;
            }
        }

        void OnTouchPerform(Finger finger)
        {

        }

        void OnTouchCanceled(Finger finger)
        {
            cinemachineController.enabled = false;
        }
    }
}
