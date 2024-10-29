using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Editor
{
    public class MapEditor
    {
        public MapConcrete mapConcrete;
        public MapObject focusObject;
        
        private List<Operations.IMapEditOperation> operationQueue;

        public void CreateObj(Vector3 position, Model model)
        {
            mapConcrete.AddMapObject(position, Quaternion.identity, model);
        }

        public void DeleteObj(MapObject mapObject)
        {
            mapConcrete.DeleteMapObject(mapObject);
        }

        public void Execute(Operations.Create oper)
        {
            throw new NotImplementedException();
        }
        
        public void Execute(Operations.Delete oper)
        {
            throw new NotImplementedException();
        }
    }
}