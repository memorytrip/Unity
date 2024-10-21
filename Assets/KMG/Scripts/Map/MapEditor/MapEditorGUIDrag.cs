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
                context.focusObject.gameObject.SetActive(true);
                context.focusObject.transform.position = QuantizatePosition(hitdata.point);
            } else {
                context.focusObject.gameObject.SetActive(false);
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            if (context.focusObject.gameObject.activeSelf) {
				context.focusObject.GetComponent<Collider>().enabled = true;
            } else {
                MonoBehaviour.Destroy(context.focusObject.gameObject);
            }
            context.SwitchState(new MapEditorGUIIdle(context));
        }

        private Vector3 QuantizatePosition(Vector3 pos)
        {
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);
            return pos;
        }
    }
}