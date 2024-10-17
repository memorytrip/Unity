using UnityEngine;



namespace Map {
    [CreateAssetMenu(fileName = "MapObjectModel", menuName = "Scriptable Objects/MapObjectModel")]
    public class MapObjectModel : ScriptableObject
    {
        public string modelId;
        public Mesh mesh;
        public Material material;
        public Vector3 size;
    }
}
