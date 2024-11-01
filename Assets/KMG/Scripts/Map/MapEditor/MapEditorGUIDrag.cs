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
                context.target.ActiveFocus();
                context.target.Move(QuantizatePosition(hitdata.point));
                
                context.rotationButton.gameObject.SetActive(true);
                Vector3 buttonPos = Camera.main.WorldToScreenPoint(context.target.GetPositionOfFocus());
                buttonPos.y += 100;
                context.rotationButton.transform.position = buttonPos;
            } else {
                context.target.DeactiveFocus();
                context.rotationButton.gameObject.SetActive(false);
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            if (context.target.GetActiveOfFocus()) {
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
    }
}