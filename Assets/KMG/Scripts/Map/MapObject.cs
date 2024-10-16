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
        public uint modelId;
        public uint objectId;
        private Mesh mesh;
        private Material material;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshRenderer>().material = material;
        }
        
        private void SetModel(uint modelId)
        {
            throw new NotImplementedException();
        }
    }

}