using System;
using System.Collections.Generic;
using Map.Editor.Operations;
using UnityEngine;

namespace Map.Editor
{
    public class MapEditor
    {
        public MapConcrete mapConcrete;
        public MapObject focusObject;

        private Stack<Operations.MapEditOperation> operationQueue;
        private Material outlineMaterial;

        public MapEditor()
        {
            operationQueue = new Stack<MapEditOperation>();
            outlineMaterial = new Material(Shader.Find("Shader Graphs/Outline"));
        }

        public bool IsFocusing() => focusObject != null;

        public void Move(Vector3 position) => focusObject.transform.position = position;
        public void Rotate(Quaternion rotation) => focusObject.transform.rotation = rotation;
        
        public MapObject Create(Vector3 position, Model model) => mapConcrete.AddMapObject(position, Quaternion.identity, model); 
        public MapObject Create(Vector3 position, Quaternion rotation, Model model)
            => mapConcrete.AddMapObject(position, rotation, model);
        public void Delete() => mapConcrete.DeleteMapObject(focusObject);
        public void ActiveFocus() => focusObject.gameObject.SetActive(true);
        public void DeactiveFocus() => focusObject.gameObject.SetActive(false);

        public void EnableCollider() => focusObject.GetComponent<Collider>().enabled = true;
        public void DisableCollider() => focusObject.GetComponent<Collider>().enabled = false;
        public Model GetModel() => focusObject.GetModel();
        public bool GetActiveOfFocus() => focusObject.gameObject.activeSelf;
        public Vector3 GetPositionOfFocus() => focusObject.transform.position;
        public Quaternion GetRotationOfFocus() => focusObject.transform.rotation;

        public void FocusOn(MapObject mapObject)
        {
            FocusOff();
            
            // 외곽선 쉐이더 적용
            // https://bloodstrawberry.tistory.com/707
            focusObject = mapObject;
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

            focusObject = null;
        }

        public void Execute(MapEditOperation oper)
        {
            Debug.Log($"push: {oper}");
            operationQueue.Push(oper);
            oper.Execute();
        }

        public void Undo()
        {

            if (operationQueue.Count > 0)
            {
                Debug.Log($"pop: {operationQueue.Peek()}");
                operationQueue.Pop().UnExecute();
            }
        }
        
    }
}