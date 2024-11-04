using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Map
{
    /**
     * MapInfo가 게임오브젝트로 구체화된 것
     */
    public class MapConcrete
    {
        private const string mapObjectPrefabPath = "Prefabs/MapObject";
        
        public MapInfo info;
        public GameObject rootObject;
        public string themeId;
        public List<MapObject> mapObjects;
        private GameObject mapObjectPrefab;

        private MeshFilter rootMeshFilter;
        private MeshRenderer rootMeshRenderer;
        private MeshCollider rootMeshCollider;

        public MapConcrete()
        {
            rootObject = new GameObject("MapConcrete");
            rootMeshFilter = rootObject.AddComponent<MeshFilter>();
            rootMeshRenderer = rootObject.AddComponent<MeshRenderer>();
            rootMeshCollider = rootObject.AddComponent<MeshCollider>();
            mapObjects = new List<MapObject>();
            mapObjectPrefab = Resources.Load<GameObject>(mapObjectPrefabPath);
        }

        /**
         * 지정된 transform에 model을 Instantiate
         */
        public MapObject AddMapObject(Vector3 position, Quaternion rotation, Model model, string guid = null)
        {
            GameObject gameObject = MonoBehaviour.Instantiate(mapObjectPrefab, rootObject.transform);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            
            MapObject mapObject = gameObject.GetComponent<MapObject>();
            mapObject.SetModel(model);
            if (guid == null)
                mapObject.id = System.Guid.NewGuid().ToString(); // 맵에디팅 위함
            else
                mapObject.id = guid;
            
            mapObjects.Add(mapObject);
            return mapObject;
        }

        public void DeleteMapObject(MapObject mapObject)
        {
            mapObjects.Remove(mapObject);
            MonoBehaviour.Destroy(mapObject.gameObject);
        }

        public async UniTaskVoid SetTheme(string themeId)
        {
            Theme theme = await ModelManager.Instance.GetTheme(themeId);
            this.themeId = themeId;
            rootMeshFilter.mesh = theme.mesh;
            rootMeshRenderer.material = theme.material;
            rootMeshCollider.sharedMesh = theme.mesh;
        }

        public void RefreshInfo()
        {
            info = MapConverter.ConvertMapConcreteToMapInfo(this);
        }
    }
}