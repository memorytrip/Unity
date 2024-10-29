using UnityEngine.InputSystem.EnhancedTouch;

namespace Map.Editor
{
    public abstract class MapEditorGUIState
    {
        enum State
        {
            Idle,
            Drag,
            Move
        }
        protected MapEditorGUI context;

        public MapEditorGUIState(MapEditorGUI context)
        {
            this.context = context;
        }

        public abstract void OnTouchStart(Finger finger);
        public abstract void OnTouchPerform(Finger finger);
        public abstract void OnTouchCanceled(Finger finger);
    }
}