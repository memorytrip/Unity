using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUIDrag: MapEditorGUIState
    {
        public MapEditorGUIDrag(MapEditorGUI context) : base(context) { }

        public override void OnTouchStart(Finger finger)
        { }

        public override void OnTouchPerform(Finger finger)
        {
            Debug.Log(context.mainCamera.gameObject.name);
            Ray mouseRay = context.mainCamera.ScreenPointToRay(finger.screenPosition);
            RaycastHit hitdata;
            if (Physics.Raycast(mouseRay, out hitdata))
            {
                context.target.focusObject.gameObject.SetActive(true);
                context.target.focusObject.transform.position = QuantizatePosition(hitdata.point);
                
                // context.rotationButton.gameObject.SetActive(true);
                // Vector3 buttonPos = Camera.main.WorldToScreenPoint(context.target.focusObject.transform.position);
                // buttonPos.y += 100;
                // context.rotationButton.transform.position = buttonPos;
            } else {
                context.target.focusObject.gameObject.SetActive(false);
                context.rotationButton.gameObject.SetActive(false);
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            if (context.target.focusObject.gameObject.activeSelf) {
				context.target.focusObject.GetComponent<Collider>().enabled = true;
            } else {
                context.target.mapConcrete.DeleteMapObject(context.target.focusObject);
            }
            context.scrollRect.horizontal = true;
            context.SwitchState(new MapEditorGUIIdle(context));
        }

        private Vector3 QuantizatePosition(Vector3 pos)
        {
            pos.x = Mathf.Round(pos.x);
            pos.y = 100; //Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);
            RaycastHit hit;
            Physics.Raycast(pos, Vector3.down, out hit, 150f);
            return hit.point;
        }
    }
}