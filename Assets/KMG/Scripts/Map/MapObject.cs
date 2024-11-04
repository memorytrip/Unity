using System;
using UnityEngine;

namespace Map
{
 
    /**
     * Map에 배치되는 Object
     */
    public class MapObject : MonoBehaviour
    {
        private Model model;
        
        /**
         * TODO: 맵에디팅에 사용할 Id 부여하기
         */
        private string _id = null;
        public string id
        {
            get { return _id; }
            set {
                if (_id == null)
                {
                    _id = value;
                }
                else
                {
                    throw new Exception("Try to set id of MapObject which already is set");
                }
            }
        }
        
        /** 
         * 생성과 함꼐 실행
         */
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