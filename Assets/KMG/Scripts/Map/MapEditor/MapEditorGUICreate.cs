using Map;
using Map.Editor;
using Map.Editor.Operations;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace KMG.Scripts.Map.MapEditor
{
    public class MapEditorGUICreate: MapEditorGUIState
    {
        public MapEditorGUICreate(MapEditorGUI context) : base(context) { }

        private Model model;

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
                context.target.Delete();
                context.target.Execute(new Create(context, context.target.GetPositionOfFocus(), model));
                context.target.EnableCollider();
            } else {
                context.target.Delete();
            }
            context.SwitchState(new MapEditorGUIIdle(context));
        }

        public MapEditorGUICreate SetModel(Model model)
        {
            this.model = model;
            return this;
        }
    }
}