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
            Ray mouseRay = Camera.main.ScreenPointToRay(finger.screenPosition);
            RaycastHit hitdata;
            if (Physics.Raycast(mouseRay, out hitdata))
            {
                context.target.focusObject.gameObject.SetActive(true);
                context.target.focusObject.transform.position = QuantizatePosition(hitdata.point);
            } else {
                context.target.focusObject.gameObject.SetActive(false);
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            if (context.target.focusObject.gameObject.activeSelf) {
				context.target.focusObject.GetComponent<Collider>().enabled = true;
            } else {
                context.target.mapConcrete.DeleteMapObject(context.target.focusObject);
            }
            context.SwitchState(new MapEditorGUIIdle(context));
        }

        private Vector3 QuantizatePosition(Vector3 pos)
        {
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y) + 0.5f;
            pos.z = Mathf.Round(pos.z);
            return pos;
        }
    }
}