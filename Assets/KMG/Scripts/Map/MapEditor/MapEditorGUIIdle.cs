using System.Collections.Generic;
using KMG.Scripts.Map.MapEditor;
using Map.Editor.Operations;
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
                    // TODO: 배치될 때 까지는 임시 오브젝트로 보여주는 게 좋겠다
                    MapEditorItem item;
                    if (result.gameObject.TryGetComponent<MapEditorItem>(out item)) {
                        // MapObject mapObject = context.target.Create(Vector3.zero, item.model);
                        // context.target.FocusOn(mapObject);
                        // context.target.DeactiveFocus();
                        // context.target.DisableCollider();
                        GameObject proto = MonoBehaviour.Instantiate(context.mapObjectProtoPrefab);
                        proto.SetActive(false);
                        proto.GetComponent<Collider>().enabled = false;
                        proto.GetComponent<MeshFilter>().mesh = item.model.mesh;
                        context.mapObjectProto = proto;
						context.SwitchState(new MapEditorGUICreate(context).SetModel(item.model));
                        break;
                    }

                    // 회전
                    if (result.gameObject == context.rotationButton)
                    {
                        context.SwitchState(
                            new MapEditorGUIRotate(context, finger.screenPosition.x)
                                .SetFrom(context.target.GetRotationOfFocus())
                        );
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
                    // context.target.FocusOn(mapObject);
                    context.target.Execute(new Focus(context, mapObject.id));
                    context.target.DisableCollider();
                    context.SwitchState(
                        new MapEditorGUIMove(context)
                            .SetFromPosition(context.target.GetPositionOfFocus())
                            .SetFromRotation(context.target.GetRotationOfFocus())
                            .SetModel(context.target.GetModel())
                        );
                    return;
                }
            }
            
            // 카메라 이동
            context.cinemachineController.enabled = true;
            context.SwitchState(new MapEditorGUICam(context));
            context.target.Execute(new Focus(context, null));
                
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