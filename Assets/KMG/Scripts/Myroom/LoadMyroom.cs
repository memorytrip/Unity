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

            MapInfo mapInfo = await LoadFromLocal();
            
            LoadMap(mapInfo).Forget();
        }

        private async UniTask<MapInfo> LoadFromLocal()
        {
            string path = Application.persistentDataPath + "/Maps/";
            string filename = "asdf.json";
            Debug.Log(path + filename);
            
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            MapInfo mapInfo = MapInfo.GetDefaultMap();;
            if (File.Exists(path + filename))
            {
                string rawData = await File.ReadAllTextAsync(path + filename);
                mapInfo = JsonConvert.DeserializeObject<MapInfo>(rawData);
            }

            return mapInfo;
        }

        private async UniTask<MapInfo> LoadFromServer()
        {
            string data = await DataManager.Get("/myroom");
            MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(data);
            return mapInfo;
        }

        private async UniTaskVoid LoadMap(MapInfo mapInfo)
        {
            MapConcrete map = await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
            map.rootObject.layer = LayerMask.NameToLayer("Ground");
            LoadWater(map).Forget();
        }

        private async UniTaskVoid LoadWater(MapConcrete mapConcrete)
        {
            await UniTask.WaitWhile(() => mapConcrete.isLoading);
            Theme theme = mapConcrete.theme;
            if (theme.water != null)
            {
                GameObject water = new GameObject("Water", typeof(MeshFilter), typeof(MeshRenderer));
                water.transform.SetParent(mapConcrete.rootObject.transform);
                water.GetComponent<MeshFilter>().mesh = theme.water.mesh;
                water.GetComponent<MeshRenderer>().material = theme.water.material;
                water.transform.position = theme.water.position;
            }
        }
    }
}