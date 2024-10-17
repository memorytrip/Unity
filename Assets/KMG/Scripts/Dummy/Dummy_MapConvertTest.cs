using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Map;

namespace KMG.Scripts.Dummy
{
    public class Dummy_MapConvertTest: MonoBehaviour
    {
        [SerializeField] private GameObject mapConcreteRoot;
        private MapConcrete mapConcrete;
        private async UniTaskVoid Start()
        {
            MapInfo mapInfo = new MapInfo();
            mapInfo.id = "0";
            mapInfo.thumbnail = null;
            mapInfo.data = "{\"themeId\":null,\"mapObjectList\":[{\"modelId\":\"0\",\"position\":{\"x\":0.0,\"y\":0.0,\"z\":0.0},\"rotation\":{\"w\":1.0,\"x\":0.0,\"y\":0.0,\"z\":0.0}},{\"modelId\":\"1\",\"position\":{\"x\":-1.0,\"y\":0.0,\"z\":0.0},\"rotation\":{\"w\":1.0,\"x\":0.0,\"y\":0.0,\"z\":0.0}}]}";
            
            // mapConcrete = new MapConcrete(mapInfo);
            // mapConcrete.rootObject = mapConcreteRoot;
            // mapConcrete.AddMapObject(Vector3.zero, Quaternion.identity, await ModelManager.Instance.Find("0"));
            // mapConcrete.AddMapObject(Vector3.left, Quaternion.identity, await ModelManager.Instance.Find("1"));
            mapConcrete = await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);

            MapInfo mapInfo2 = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete);
            Debug.Log(mapInfo2.data);
        }
    }
}