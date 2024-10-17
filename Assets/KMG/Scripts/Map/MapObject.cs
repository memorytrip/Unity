using System;
using UnityEngine;

namespace Map
{
 
    /**
     * Map에 배치되는 Object
     * TODO: SetModel 구현
     */
    public class MapObject : MonoBehaviour
    {
        public string id
        {
            get { return _id; }
        }
        private string _id;
        private Model model;

        private void Start()
        {
            _id = System.Guid.NewGuid().ToString();
        }

        public void SetModel(Model model)
        {
            this.model = model;
            GetComponent<MeshFilter>().mesh = model.mesh;
            GetComponent<MeshRenderer>().material = model.material;
        }

        public Model GetModel()
        {
            return model;
        }
    }

}