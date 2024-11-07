using UnityEngine;
using UnityEngine.Serialization;

namespace Map {
    [CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects/Theme")]
    public class Theme : ScriptableObject
    {
        public string id;
        public Mesh mesh;
        public Material material;
        public Water water;
    }
}
