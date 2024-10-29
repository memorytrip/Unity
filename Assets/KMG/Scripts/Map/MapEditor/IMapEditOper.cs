using UnityEngine;

namespace Map.Editor.Operations
{
    public interface IMapEditOperation
    {
        public void Execute(MapEditor context);
        public void UnExecute(MapEditor context);
    }

    public class Move : IMapEditOperation
    {
        private MapObject target;
        private Vector3 from;
        private Vector3 to;

        public Move(MapObject target, Vector3 to)
        {
            this.target = target;
            this.from = target.transform.position;
            this.to = to;
        }
        
        public void Execute(MapEditor context)
        {
            target.transform.position = to;
        }

        public void UnExecute(MapEditor context)
        {
            target.transform.position = from;
        }
    }
    
    public class Rotate : IMapEditOperation
    {
        private MapObject target;
        private Quaternion from;
        private Quaternion to;

        public Rotate(MapObject target, Quaternion to)
        {
            this.target = target;
            this.from = target.transform.rotation;
            this.to = to;
        }
        
        public void Execute(MapEditor context)
        {
            target.transform.rotation = to;
        }

        public void UnExecute(MapEditor context)
        {
            target.transform.rotation = from;
        }
    }

    public class Create : IMapEditOperation
    {
        private Model model;
        private Vector3 position;
        private Quaternion rotation;
        private MapObject target;

        public Create(Vector3 position, Quaternion rotation, Model model)
        {
            this.model = model;
            this.position = position;
            this.rotation = rotation;
        }
        
        public void Execute(MapEditor context)
        {
            context.CreateObj(position, model);
        }

        public void UnExecute(MapEditor context)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class Delete: IMapEditOperation
    {
        private Model model;
        private Vector3 position;
        private Quaternion rotation;
        
        public void Execute(MapEditor context)
        {
            throw new System.NotImplementedException();
        }

        public void UnExecute(MapEditor context)
        {
            throw new System.NotImplementedException();
        }
    }
}
// TODO: undo 구현하기
// 맵오브젝트마다 uuid 부여해서 각 operation이 mapobject를 직접 참조하는 게 아니라 uuid를 통해서 참조하도록...
// 그렇게 해야 delete undo가 가능해질 것 같다.