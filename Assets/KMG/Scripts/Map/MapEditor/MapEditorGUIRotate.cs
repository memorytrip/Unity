using Map.Editor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace KMG.Scripts.Map.MapEditor
{
    public class MapEditorGUIRotate: MapEditorGUIState
    {
        private float startX = 0f;
        private const float unitX = 100;

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
            context.target.focusObject.transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
        }

        public override void OnTouchCanceled(Finger finger)
        {
            context.SwitchState(new MapEditorGUIIdle(context));
        }
    }
}