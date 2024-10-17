using UnityEngine;

namespace Map {
    /**
     * MapObject가 될 Model의 데이터를 저장하는 ScriptableObject
     */
    [CreateAssetMenu(fileName = "MapObjectModel", menuName = "Scriptable Objects/MapObjectModel")]
    public class MapObjectModel : ScriptableObject
    {
        public string modelId;
        public Mesh mesh;
        public Material material;
        public Vector3 size;
    }
}
