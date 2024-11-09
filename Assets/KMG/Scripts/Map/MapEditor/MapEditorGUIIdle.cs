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
                    
                    // 테마 설정
                    MapEditorThemeItem themeItem;
                    if (result.gameObject.TryGetComponent<MapEditorThemeItem>(out themeItem))
                    {
                        Theme theme = themeItem.theme;
                        context.changeThemePanel.SetActive(true);
                        context.changeThemeOKButton.onClick.RemoveAllListeners();
                        context.changeThemeOKButton.onClick.AddListener(()=>
                        {
                            context.target.SetTheme(theme);
                            context.changeThemePanel.SetActive(false);
                        });
                        // context.target.SetTheme(theme);
                        break;
                    }
                    
                    
                    // 새 오브젝트 생성
                    MapEditorModelItem modelItem;
                    if (result.gameObject.TryGetComponent<MapEditorModelItem>(out modelItem)) {
                        MapObject mapObject = context.target.mapConcrete.AddMapObject(Vector3.zero, Quaternion.identity, modelItem.model);
                        SelectObject(mapObject);
                        context.target.focusObject.gameObject.SetActive(false);
                        context.scrollRect.horizontal = false;
						context.SwitchState(new MapEditorGUIDrag(context));
                        break;
                    }

                    // 회전
                    // if (result.gameObject.TryGetComponent<Button>(out button))
                    if (result.gameObject.CompareTag("MapEditRotationButton"))
                    {
                        context.SwitchState(new MapEditorGUIRotate(context, finger.screenPosition.x));
                    }
                }
                return;
            }

            // 배치된 오브젝트 선택
            Ray mouseRay = context.mainCamera.ScreenPointToRay(finger.screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                Debug.Log(hit.transform.name);
                MapObject mapObject;
                if (hit.transform.TryGetComponent(out mapObject))
                {
                    SelectObject(mapObject);
                    context.SwitchState(new MapEditorGUIDrag(context));
                    return;
                }
            }
            
            // 카메라 이동
            context.cinemachineController.enabled = true;
            context.SwitchState(new MapEditorGUICam(context));
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