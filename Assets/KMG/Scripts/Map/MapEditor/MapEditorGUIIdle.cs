using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

namespace Map.Editor
{
    public class MapEditorGUIIdle: MapEditorGUIState
    {
        public MapEditorGUIIdle(MapEditorGUI context) : base(context) { }
        
        public override void OnTouchStart(Finger finger)
        {
            if (GUI.Utility.IsPointOverGUI(finger.screenPosition)) {
                List<RaycastResult> results = GUI.Utility.RaycastWithPoint(finger.screenPosition);
                
                foreach (var result in results) {
                    
                    // 새 오브젝트 생성
                    MapEditorItem item;
                    if (result.gameObject.TryGetComponent<MapEditorItem>(out item)) {
                        context.target.Create(Vector3.zero, item.model);
                        context.target.ActiveFocus();
                        context.target.DisableCollider();
						context.SwitchState(new MapEditorGUIDrag(context));
                        break;
                    }

                    // 회전
                    Button button;
                    if (result.gameObject.TryGetComponent<Button>(out button))
                    {
                        context.SwitchState(new MapEditorGUIRotate(context, finger.screenPosition.x));
                    }
                }
                return;
            }

            // 배치된 오브젝트 선택
            Ray mouseRay = Camera.main.ScreenPointToRay(finger.screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                Debug.Log(hit.transform.name);
                MapObject mapObject;
                if (hit.transform.TryGetComponent(out mapObject))
                {
                    context.target.FocusOn(mapObject);
                    context.target.DisableCollider();
                    context.SwitchState(new MapEditorGUIDrag(context));
                    return;
                }
            }
            
            // 카메라 이동
            context.cinemachineController.enabled = true;
            context.SwitchState(new MapEditorGUIMove(context));
            context.target.FocusOff();
                
        }

        public override void OnTouchPerform(Finger finger) { }

        public override void OnTouchCanceled(Finger finger) { }

        private void SelectObject(MapObject mapObject)
        {
            context.target.FocusOn(mapObject);
            mapObject.GetComponent<Collider>().enabled = false;
        }
    }
}