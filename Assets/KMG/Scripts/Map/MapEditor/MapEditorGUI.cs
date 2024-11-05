using System;
using System.Collections;
using System.IO;
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
        [SerializeField] private CinemachineCamera cam;
        [SerializeField] private Transform camTarget;
        [SerializeField] public CinemachineInputAxisController cinemachineController;
        public MapEditor target;
        [SerializeField] public Button rotationButton;
        private MapEditorGUIState state;
        
        [Header("ItemList")]
        [SerializeField] private Transform ItemList;
        [SerializeField] public ScrollRect scrollRect;
        [SerializeField] private GameObject ModelItemPrefab;
        [SerializeField] private RenderTexture itemRenderTexture;
        
        void Start()
        {
            target = new MapEditor();
            InputManager.Instance.OnFingerDown += OnTouchStart;
            InputManager.Instance.OnFingerMove += OnTouchPerform;
            InputManager.Instance.OnFingerUp += OnTouchCanceled;
            InputManager.Instance.OnPinch += OnZoom;
            InputManager.Instance.OnPanning += OnPanningCam;
            cinemachineController.enabled = false;
            rotationButton.gameObject.SetActive(false);
            state = new MapEditorGUIIdle(this);
            RefreshItemList();
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

        void OnZoom(object sender, PinchEventArgs args)
        {
            var diff = -args.difference / 100;
            cam.Lens.OrthographicSize = Mathf.Clamp(cam.Lens.OrthographicSize + diff, 0.5f, 50f);
        }

        void OnPanningCam(object sender, PanningEventArgs args)
        {
            var diff = - args.difference;
            var center = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            var ray = Camera.main.ScreenPointToRay(center + diff);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Water")))
            {
                var point = hit.point;
                Debug.Log($"{center + diff}, {point}");
            
                var toPosition = new Vector3(point.x, point.y, point.z);
                toPosition.x = Mathf.Clamp(point.x, -20f, 20f);
                toPosition.z = Mathf.Clamp(point.z, -20f, 20f);
                camTarget.position = toPosition;
            }
        }

        public void SwitchState(MapEditorGUIState state)
        {
            this.state = state;
        }

        private void Update()
        {
            UpdateRotationIcon();
        }

        private void UpdateRotationIcon()
        {
            if (target.focusObject != null)
            {
                rotationButton.gameObject.SetActive(true);
                // Vector3 buttonPos = Camera.main.WorldToScreenPoint(target.focusObject.transform.position);
                Vector3 buttonPos = target.focusObject.transform.position;
                buttonPos.y += 10;
                rotationButton.transform.position = buttonPos;
            }
            else
            {
                rotationButton.gameObject.SetActive(false);
            }
        }

        private void RefreshItemList()
        {
            foreach (Transform child in ItemList)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var model in ModelManager.Instance.downloadModelList)
            {
                var item = Instantiate(ModelItemPrefab, ItemList);
                item.GetComponent<MapEditorItem>().model = model;
                item.GetComponent<MapEditorItem>().InitThumbnail();
            }
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnFingerDown -= OnTouchStart;
            InputManager.Instance.OnFingerMove -= OnTouchPerform;
            InputManager.Instance.OnFingerUp -= OnTouchCanceled;
            InputManager.Instance.OnPinch -= OnZoom;
            InputManager.Instance.OnPanning -= OnPanningCam;
        }
    }
}
