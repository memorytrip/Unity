using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Map;

namespace Myroom
{
    public class LoadMyroom: MonoBehaviour
    {
        private async UniTaskVoid Start()
        {
            List<MapInfo> mapInfos = await MapManager.Instance.LoadMapList(new User());
            MapConcrete map = await MapManager.Instance.LoadMap(mapInfos[0]);
            map.rootObject.layer = LayerMask.NameToLayer("Ground");
        }
    }
}