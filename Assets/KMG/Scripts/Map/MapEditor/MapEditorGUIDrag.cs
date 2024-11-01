using Map.Editor.Operations;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUIDrag: MapEditorGUIState
    {
        private Vector3 fromPosition;
        public MapEditorGUIDrag(MapEditorGUI context) : base(context) { }

        public override void OnTouchStart(Finger finger)
        { }

        public override void OnTouchPerform(Finger finger)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(finger.screenPosition);
            RaycastHit hitdata;
            if (Physics.Raycast(mouseRay, out hitdata))
            {
                context.target.ActiveFocus();
                context.target.Move(QuantizatePosition(hitdata.point));
            } else {
                context.target.DeactiveFocus();
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            if (context.target.GetActiveOfFocus()) {
                context.target.Execute(new Move(context, fromPosition, context.target.GetPositionOfFocus()));
				context.target.EnableCollider();
            } else {
                context.target.Delete();
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

        public MapEditorGUIDrag SetFrom(Vector3 position)
        {
            fromPosition = position;
            return this;
        }
    }
}