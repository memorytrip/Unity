
using Map.Editor.Operations;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public class MapEditorGUIRotate: MapEditorGUIState
    {
        private float startX = 0f;
        private const float unitX = 100;
        private Quaternion fromRotation;
        private Quaternion toRotation;

        public MapEditorGUIRotate(MapEditorGUI context, float startX = 0f) : base(context)
        {
            this.startX = startX;
        }

        public override void OnTouchStart(Finger finger)
        {

        }

        public override void OnTouchPerform(Finger finger)
        {
            float deltaX = finger.screenPosition.x - startX;
            float rotateY = -Mathf.Round(deltaX / unitX) * 15;
            toRotation = Quaternion.Euler(0f, rotateY, 0f);
            context.target.Rotate(toRotation);
        }

        public override void OnTouchCanceled(Finger finger)
        {
            context.target.Execute(new Rotate(context, fromRotation, toRotation));
            context.SwitchState(new MapEditorGUIIdle(context));
        }

        public MapEditorGUIRotate SetFrom(Quaternion from)
        {
            fromRotation = from;
            return this;
        }
    }
}