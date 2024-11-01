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
        private MapObject from;
        private MapObject to;

        public Focus(MapEditorGUI context, MapObject to) : base(context)
        {
            this.from = target.focusObject;
            this.to = to;
        }

        public override void Execute()
        { 
            target.FocusOff();
            if (to != null) target.FocusOn(to);
        }

        public override void UnExecute()
        {
            target.FocusOff();
            if (from != null) target.FocusOn(from);
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
            context.target.FocusOn(target.Create(position, model));
        }

        public override void UnExecute()
        {
            target.Delete();
            context.target.FocusOn(fromFocus);
        }
    }

    public class Remove : MapEditOperation
    {
        private Vector3 position;
        private Quaternion rotation;
        private Model model;
        public Remove(MapEditorGUI context, Vector3 position, Quaternion rotation, Model model) : base(context)
        {
            this.position = position;
            this.rotation = rotation;
            this.model = model;
        }

        public override void Execute()
        {
            target.Delete();
        }

        public override void UnExecute()
        {
            target.Create(position, rotation, model);
        }
    }
}
// TODO: undo 구현하기
// 맵오브젝트마다 uuid 부여해서 각 operation이 mapobject를 직접 참조하는 게 아니라 uuid를 통해서 참조하도록...
// 그렇게 해야 delete undo가 가능해질 것 같다.