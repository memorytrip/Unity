using UnityEngine;
using UnityEngine.Serialization;

namespace Map {
    [CreateAssetMenu(fileName = "Water", menuName = "Scriptable Objects/Water")]
    public class Water : ScriptableObject
    {
        public string themeId;
        public Mesh mesh;
        public Material material;
        public Vector3 position;
    }
}
