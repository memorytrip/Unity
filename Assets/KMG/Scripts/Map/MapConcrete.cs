using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapConcrete
    {
        private const string mapObjectPrefabPath = "KMG/Prefabs/MapObject.prefab";
        
        public MapInfo info;
        public GameObject rootObject;
        private List<MapObject> mapObjects;
        private GameObject mapObjectPrefab;

        public MapConcrete(MapInfo info)
        {
            this.info = info;
            rootObject = new GameObject("MapConcrete");
            mapObjects = new List<MapObject>();
            mapObjectPrefab = Resources.Load<GameObject>(mapObjectPrefabPath);
        }

        public void AddMapObject(Vector3 position, Quaternion rotation, MapObjectModel model)
        {
            GameObject gameObject = MonoBehaviour.Instantiate(mapObjectPrefab);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            
            MapObject mapObject = gameObject.GetComponent<MapObject>();
            mapObject.SetModel(model);
            
            mapObjects.Add(mapObject);
        }

        public void DeleteMapObject(MapObject mapObject)
        {
            mapObjects.Remove(mapObject);
            MonoBehaviour.Destroy(mapObject);
        }

        public void RefreshInfo()
        {
            
        }
    }
}