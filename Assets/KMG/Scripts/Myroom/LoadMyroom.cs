using System.Collections.Generic;
using System.IO;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Map;
using Newtonsoft.Json;

namespace Myroom
{
    public class LoadMyroom: MonoBehaviour
    {
        private async UniTaskVoid Start()
        {
            // TODO: BE에서 mapInfo 불러와서 하기
            // List<MapInfo> mapInfos = await MapManager.Instance.LoadMapList(new User());
            // MapConcrete map = await MapManager.Instance.LoadMap(mapInfos[0]);
            
            string path = Application.persistentDataPath + "/Maps/";
            string filename = "asdf.json";
            
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            MapInfo mapInfo;
            if (File.Exists(path + filename))
            {
                string rawData = await File.ReadAllTextAsync(path + filename);
                mapInfo = JsonConvert.DeserializeObject<MapInfo>(rawData);
            }
            else
            {
                mapInfo = MapInfo.GetDefaultMap();
            }
            Debug.Log(mapInfo);
            MapConcrete map = await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
            map.rootObject.layer = LayerMask.NameToLayer("Ground");
        }
    }
}