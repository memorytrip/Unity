using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    /**
     * 카메라 움직이는 상태
     */
    public class MapEditorGUIMove: MapEditorGUIState
    {
        public MapEditorGUIMove(MapEditorGUI context) : base(context) { }

        public override void OnTouchStart(Finger finger)
        {
            
        }

        public override void OnTouchPerform(Finger finger)
        {
            
        }

        public override void OnTouchCanceled(Finger finger)
        {
            context.cinemachineController.enabled = false;
            context.SwitchState(new MapEditorGUIIdle(context));
        }
    }
}