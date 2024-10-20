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
                context.focusObject.transform.position = hitdata.point;
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
    }
}