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
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitdata;
            if (Physics.Raycast(mouseRay, out hitdata))
            {
                context.focusObject.transform.position = hitdata.point;
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            context.SwitchState(new MapEditorGUIIdle(context));
            context.focusObject.GetComponent<Collider>().enabled = true;
        }
    }
}