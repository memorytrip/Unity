using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Editor
{
    public class MapEditor
    {
        public MapConcrete mapConcrete;
        private MapObject _focusObject;
        public MapObject focusObject { get => _focusObject; }

        private List<Operations.IMapEditOperation> operationQueue;
        private Material outlineMaterial;

        public MapEditor()
        {
            outlineMaterial = new Material(Shader.Find("Shader Graphs/Outline"));
        }

        public void CreateObj(Vector3 position, Model model)
        {
            mapConcrete.AddMapObject(position, Quaternion.identity, model);
        }

        public void DeleteObj(MapObject mapObject)
        {
            mapConcrete.DeleteMapObject(mapObject);
        }

        public void FocusOn(MapObject mapObject)
        {
            FocusOff();
            
            // 외곽선 쉐이더 적용
            // https://bloodstrawberry.tistory.com/707
            _focusObject = mapObject;
            Renderer renderer = focusObject.GetComponent<Renderer>();
            
            List<Material> materialList = new List<Material>();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Add(outlineMaterial);
            
            renderer.materials = materialList.ToArray();
        }

        public void FocusOff()
        {
            if (focusObject == null) return;
            
            // 외곽선 쉐이더 해제
            Renderer renderer = focusObject.GetComponent<Renderer>();
            
            List<Material> materialList = new List<Material>();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Remove(outlineMaterial);
            
            renderer.materials = materialList.ToArray();

            _focusObject = null;
        }
    }
}