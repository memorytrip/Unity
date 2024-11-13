using System.Collections.Generic;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Map;
using UnityEngine;


namespace GUI
{
    public class InMyroomMapList : MonoBehaviour
    {
        [SerializeField] private GameObject defaultMapItemPrefab;
        [SerializeField] private GameObject customMapItemPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Sprite emptySprite;

        public async UniTask RefreshListProcess()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            User user = SessionManager.Instance.currentUser;
            List<MapInfo> mapInfos = await MapManager.Instance.LoadMapList(user);

            List<UniTask> tasks = new List<UniTask>();
            foreach (var mapInfo in mapInfos)
            {
                if (mapInfo.type == MapInfo.MapType.Default)
                    tasks.Add(InitDefaultItem(mapInfo));
                else if (mapInfo.type == MapInfo.MapType.Custom)
                    tasks.Add(InitCustomMapItem(mapInfo));
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask<MapListDefaultItem> InitDefaultItem(MapInfo mapInfo)
        {
            MapListDefaultItem item = Instantiate(defaultMapItemPrefab, content).GetComponent<MapListDefaultItem>();
            item.mapId = mapInfo.id;
            Sprite thumbnail = await MapManager.Instance.LoadMapThumbnail(mapInfo);
            if (thumbnail == null)
                thumbnail = emptySprite;
            item.SetThumbnail(thumbnail);
            return item;
        }

        public async UniTask<MapListCustomItem> InitCustomMapItem(MapInfo mapInfo)
        {
            MapListCustomItem item = Instantiate(customMapItemPrefab, content).GetComponent<MapListCustomItem>();
            item.mapId = mapInfo.id;
            item.mapList = this;
            Sprite thumbnail = await MapManager.Instance.LoadMapThumbnail(mapInfo);
            if (thumbnail == null)
                thumbnail = emptySprite;
            item.SetThumbnail(thumbnail);
            return item;
        }
    }
}