using UnityEngine;

namespace Map.Editor.Operations
{
    // public interface IMapEditOperation
    public abstract class MapEditOperation
    {
        
        protected MapEditor target;
        protected MapEditorGUI context;
        public MapEditOperation(MapEditor target, MapEditorGUI context)
        {
            this.target = target;
            this.context = context;
        }
        
        public abstract void Execute(MapEditor context);
        public abstract void UnExecute(MapEditor context);
    }
    
    public class Move: MapEditOperation
    {
        
        public Move(MapEditor target, MapEditorGUI context) : base(target, context)
        {
        }

        public override void Execute(MapEditor context)
        {
            
        }

        public override void UnExecute(MapEditor context)
        {
            throw new System.NotImplementedException();
        }
    }
}
// TODO: undo 구현하기
// 맵오브젝트마다 uuid 부여해서 각 operation이 mapobject를 직접 참조하는 게 아니라 uuid를 통해서 참조하도록...
// 그렇게 해야 delete undo가 가능해질 것 같다.