using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUIIdle: MapEditorGUIState
    {
        public MapEditorGUIIdle(MapEditorGUI context) : base(context) { }
        
        public override void OnTouchStart(Finger finger)
        {
            if (GUI.Utility.IsPointOverGUI(finger.screenPosition)) {
                List<RaycastResult> results = GUI.Utility.RaycastWithPoint(finger.screenPosition);
                MapEditorItem item;
                foreach (var result in results) {
                    if (result.gameObject.TryGetComponent<MapEditorItem>(out item)) {
                        MapObject mapObject = context.target.mapConcrete.AddMapObject(Vector3.zero, Quaternion.identity, item.model);
                        context.target.focusObject = mapObject;
						mapObject.GetComponent<Collider>().enabled = false;
                        context.target.focusObject.gameObject.SetActive(false);
						context.SwitchState(new MapEditorGUIDrag(context));
                        break;
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
                    context.target.focusObject = mapObject;
                    mapObject.GetComponent<Collider>().enabled = false;
                    context.SwitchState(new MapEditorGUIDrag(context));
                    return;
                }
            }
            
            // 카메라 이동
            context.cinemachineController.enabled = true;
            context.SwitchState(new MapEditorGUIMove(context));
                
        }

        public override void OnTouchPerform(Finger finger) { }

        public override void OnTouchCanceled(Finger finger) { }
    }
}