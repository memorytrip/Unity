using System.Collections.Generic;
using UnityEngine;

namespace Map.Editor
{
    public class MapEditor: MonoBehaviour
    {
        public MapConcrete mapConcrete;
        [HideInInspector] public MapObject focusObject;
        
        private List<Operations.IMapEditOperation> operationQueue;

        public void CreateObj(Vector3 position, Model model)
        {
            mapConcrete.AddMapObject(position, Quaternion.identity, model);
        }

        public void DeleteObj(MapObject mapObject)
        {
            mapConcrete.DeleteMapObject(mapObject);
        }
    }
}