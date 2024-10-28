using Cysharp.Threading.Tasks;
using UnityEngine;
using Map;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
using Map.Editor;
using Unity.Cinemachine;
using File = System.IO.File;

namespace KMG.Scripts.Dummy
{
    public class Dummy_MapEditTest: MonoBehaviour
    {
        private const string defaultMapInfo =
            "{\"formatVersion\":\"0.0.1\",\"id\":null,\"thumbnail\":null,\"data\":\"{\\\"themeId\\\":\\\"0\\\",\\\"mapObjectList\\\":[]}\"}";
        [SerializeField] private GameObject mapConcreteRoot;
        [SerializeField] private Map.Editor.MapEditorGUI mapEditorGUI;
        [SerializeField] private Button mapConvertButton;
        [SerializeField] private ThumbnailCapture capturer;
        [SerializeField] private CinemachineCamera cam;
        private MapConcrete mapConcrete;
        private async UniTaskVoid Start()
        {
            MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(defaultMapInfo);
            
            // mapConcrete = new MapConcrete();
            // mapConcrete.rootObject = mapConcreteRoot;
            // mapConcrete.AddMapObject(Vector3.zero, Quaternion.identity, await ModelManager.Instance.Get("2"));
            // mapConcrete.AddMapObject(Vector3.left, Quaternion.identity, await ModelManager.Instance.Find("1"));
            mapConcrete = await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
            mapEditorGUI.target.mapConcrete = mapConcrete;
            cam.Target.TrackingTarget = mapConcrete.rootObject.transform;

            MapInfo mapInfo2 = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete);
            Debug.Log(mapInfo2.data);

            mapConvertButton.onClick.AddListener(()=>ConvertMap().Forget());
        }

        private async UniTaskVoid ConvertMap()
        {
            MapInfo mapInfo = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete);
            mapInfo.thumbnail = await capturer.CaptureToBase64();
            
            string path = Application.persistentDataPath + "/Maps/";
            string filename = "asdf.json";
            Debug.Log(path + filename);
            DirectoryInfo directoryInfo = new DirectoryInfo($"{Application.persistentDataPath}/Maps/");
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            await File.WriteAllTextAsync(path + filename, JsonConvert.SerializeObject(mapInfo));
        }
    }
}