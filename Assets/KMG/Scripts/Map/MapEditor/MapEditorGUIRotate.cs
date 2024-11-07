
using Map.Editor.Operations;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUIRotate: MapEditorGUIState
    {
        private float startX = 0f;
        private float startR = 0f;
        private const float unitX = 100;
        private Operations.Rotate command;

        public MapEditorGUIRotate(MapEditorGUI context, float startX = 0f) : base(context)
        {
            this.startX = startX;
            startR = context.target.focusObject.transform.rotation.eulerAngles.y;
            command = new Rotate(context.target.focusObject, context.target.focusObject.transform.rotation);
        }

        public override void OnTouchStart(Finger finger)
        {

        }

        public override void OnTouchPerform(Finger finger)
        {
            float deltaX = finger.screenPosition.x - startX;
            float rotateY = -Mathf.Round(deltaX / unitX) * 15;
            context.target.focusObject.transform.rotation = Quaternion.Euler(0f, startR + rotateY, 0f);
        }

        public override void OnTouchCanceled(Finger finger)
        {
            context.SwitchState(new MapEditorGUIIdle(context));
        }
    }
}