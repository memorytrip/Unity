using Map.Editor.Operations;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUIMove: MapEditorGUIState
    {
        private Vector3 fromPosition;
        private Quaternion fromRotation;
        private Model fromModel;
        public MapEditorGUIMove(MapEditorGUI context) : base(context) { }

        public override void OnTouchStart(Finger finger) { }

        public override void OnTouchPerform(Finger finger)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(finger.screenPosition);
            RaycastHit hitdata;
            if (Physics.Raycast(mouseRay, out hitdata))
            {
                context.target.ActiveFocus();
                context.target.Move(MapEditorGUI.QuantizatePosition(hitdata.point));
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
                context.target.Execute(new Remove(context, fromPosition, fromRotation, fromModel));
            }
            context.SwitchState(new MapEditorGUIIdle(context));
        }

        public MapEditorGUIMove SetFromPosition(Vector3 position)
        {
            fromPosition = position;
            return this;
        }
        
        public MapEditorGUIMove SetFromRotation(Quaternion rotation)
        {
            fromRotation = rotation;
            return this;
        }
        
        public MapEditorGUIMove SetModel(Model model)
        {
            fromModel = model;
            return this;
        }
    }
}