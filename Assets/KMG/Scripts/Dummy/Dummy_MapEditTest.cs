using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Map;
using Newtonsoft.Json;
using UnityEngine.UI;

namespace KMG.Scripts.Dummy
{
    public class Dummy_MapEditTest: MonoBehaviour
    {
        private const string defaultMapInfo =
            "{\"formatVersion\":\"0.0.1\",\"id\":null,\"thumbnail\":null,\"data\":\"{\\\"themeId\\\":\\\"0\\\",\\\"mapObjectList\\\":[]}\"}";
        [SerializeField] private GameObject mapConcreteRoot;
        [SerializeField] private Map.Editor.MapEditorGUI mapEditorGUI;
        [SerializeField] private Button mapConvertButton;
        private MapConcrete mapConcrete;
        private async UniTaskVoid Start()
        {
            MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(defaultMapInfo);
            
            // mapConcrete = new MapConcrete();
            // mapConcrete.rootObject = mapConcreteRoot;
            // mapConcrete.AddMapObject(Vector3.zero, Quaternion.identity, await ModelManager.Instance.Get("2"));
            // mapConcrete.AddMapObject(Vector3.left, Quaternion.identity, await ModelManager.Instance.Find("1"));
            mapConcrete = await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
            mapEditorGUI.mapConcrete = mapConcrete;

            MapInfo mapInfo2 = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete);
            Debug.Log(mapInfo2.data);

            mapConvertButton.onClick.AddListener(ConvertMap);
        }

        private void ConvertMap()
        {
            MapInfo mapInfo = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete);
            Debug.Log(JsonConvert.SerializeObject(mapInfo));
        }
    }
}