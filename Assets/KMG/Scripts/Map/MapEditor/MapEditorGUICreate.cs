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
                // context.target.ActiveFocus();
                // context.target.Move(MapEditorGUI.QuantizatePosition(hitdata.point));
                context.mapObjectProto.SetActive(true);
                context.mapObjectProto.transform.position = MapEditorGUI.QuantizatePosition(hitdata.point);
            } 
            else 
            {
                // context.target.DeactiveFocus();
                context.mapObjectProto.SetActive(false);
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            if (context.mapObjectProto.activeSelf) {
                context.target.Execute(new Create(context, context.mapObjectProto.transform.position, model));
                // context.target.EnableCollider();
            }
            MonoBehaviour.Destroy(context.mapObjectProto);
            context.SwitchState(new MapEditorGUIIdle(context));
        }

        public MapEditorGUICreate SetModel(Model model)
        {
            this.model = model;
            return this;
        }
    }
}