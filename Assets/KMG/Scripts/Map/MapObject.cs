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