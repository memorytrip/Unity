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

        public MapConcrete()
        {
            rootObject = new GameObject("MapConcrete");
            mapObjects = new List<MapObject>();
            mapObjectPrefab = Resources.Load<GameObject>(mapObjectPrefabPath);
        }

        /**
         * 지정된 transform에 model을 Instantiate
         */
        public MapObject AddMapObject(Vector3 position, Quaternion rotation, Model model)
        {
            GameObject gameObject = MonoBehaviour.Instantiate(mapObjectPrefab, rootObject.transform);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            
            MapObject mapObject = gameObject.GetComponent<MapObject>();
            mapObject.SetModel(model);
            
            mapObjects.Add(mapObject);
            return mapObject;
        }

        public void DeleteMapObject(MapObject mapObject)
        {
            mapObjects.Remove(mapObject);
            MonoBehaviour.Destroy(mapObject);
        }

        public void RefreshInfo()
        {
            info = MapConverter.ConvertMapConcreteToMapInfo(this);
        }
    }
}