using UnityEngine;
using UnityEngine.Serialization;

namespace Map {
    /**
     * MapObject가 될 Model의 데이터를 저장하는 ScriptableObject
     */
    [CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects/Theme")]
    public class Theme : ScriptableObject
    {
        public string id;
        public Mesh mesh;
        public Material material;
    }
}
