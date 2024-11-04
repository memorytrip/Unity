using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    /**
     * 카메라 움직이는 상태
     */
    public class MapEditorGUICam: MapEditorGUIState
    {
        public MapEditorGUICam(MapEditorGUI context) : base(context) { }

        public override void OnTouchStart(Finger finger)
        {
            
        }

        public override void OnTouchPerform(Finger finger)
        {
            if (Touch.activeFingers.Count == 1)
            {
                context.cinemachineController.enabled = true;
            }
            else
            {
                context.cinemachineController.enabled = false;
            }
        }

        public override void OnTouchCanceled(Finger finger)
        {
            context.cinemachineController.enabled = false;
            context.SwitchState(new MapEditorGUIIdle(context));
        }
    }
}