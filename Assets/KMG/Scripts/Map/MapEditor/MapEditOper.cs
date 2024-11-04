using UnityEngine;

namespace Map.Editor.Operations
{
    public abstract class MapEditOperation
    {
        
        protected MapEditor target;
        protected MapEditorGUI context;
        public MapEditOperation(MapEditorGUI context)
        {
            this.context = context;
            this.target = context.target;
        }
        
        public abstract void Execute();
        public abstract void UnExecute();
    }

    public class Focus : MapEditOperation
    {
        private string fromGuid;
        private string toGuid;

        public Focus(MapEditorGUI context, string to) : base(context)
        {
            this.fromGuid = target.focusObject?.id;
            this.toGuid = to;
        }

        public override void Execute()
        { 
            target.FocusOff();
            if (toGuid != null) target.FocusOn(target.FindObjectByGuid(toGuid));
        }

        public override void UnExecute()
        {
            target.FocusOff();
            if (fromGuid != null) target.FocusOn(target.FindObjectByGuid(fromGuid));
        }
        
    }
    
    public class Move: MapEditOperation
    {
        private Vector3 from;
        private Vector3 to;
        public Move(MapEditorGUI context, Vector3 from, Vector3 to) : base(context)
        {
            this.from = from;
            this.to = to;
        }

        public override void Execute()
        {
            target.Move(to);
        }

        public override void UnExecute()
        {
            target.Move(from);
        }
    }

    public class Rotate : MapEditOperation
    {
        private Quaternion from;
        private Quaternion to;
        public Rotate(MapEditorGUI context, Quaternion from, Quaternion to) : base(context)
        {
            this.from = from;
            this.to = to;
        }

        public override void Execute()
        {
            target.Rotate(to);
        }

        public override void UnExecute()
        {
            target.Rotate(from);
        }
    }

    public class Create : MapEditOperation
    {
        private string guid;
        private MapObject fromFocus;
        private Vector3 position;
        private Model model;
        public Create(MapEditorGUI context, Vector3 position, Model model) : base(context)
        {
            this.position = position;
            this.model = model;
        }

        public override void Execute()
        {
            fromFocus = context.target.focusObject;
            context.target.FocusOn(target.Create(position, Quaternion.identity, model, guid));
            guid = context.target.focusObject.id;
        }

        public override void UnExecute()
        {
            target.Delete();
            context.target.FocusOn(fromFocus);
        }
    }

    public class Remove : MapEditOperation
    {
        private string guid;
        private Vector3 position;
        private Quaternion rotation;
        private Model model;
        public Remove(MapEditorGUI context, Vector3 position, Quaternion rotation, Model model) : base(context)
        {
            this.position = position;
            this.rotation = rotation;
            this.model = model;
            guid = context.target.focusObject.id;
        }

        public override void Execute()
        {
            target.Delete();
        }

        public override void UnExecute()
        {
            context.target.FocusOn(target.Create(position, rotation, model, guid));
        }
    }
}