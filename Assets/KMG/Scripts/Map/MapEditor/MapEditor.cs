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

            _focusObject = mapObject;
            focusObject.GetComponent<Outline>().enabled = true;
        }

        public void FocusOff()
        {
            if (focusObject == null) return;
            
            focusObject.GetComponent<Outline>().enabled = false;
            _focusObject = null;
        }

        public void SetTheme(Theme theme)
        {
            mapConcrete.SetTheme(theme.id).Forget();
        }
    }
}