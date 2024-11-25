using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Fusion;
using Map;
using UnityEngine;

public class LoadFindPhotoMap : NetworkBehaviour
{
    [SerializeField] private Transform mapsObject;

    [HideInInspector] public bool isLoading;
    public static List<string> playerIds;
    public static Dictionary<string, MapId> maps;
    public override void Spawned()
    {
        isLoading = true;
        LoadMaps().Forget();
    }

    private async UniTaskVoid LoadMaps()
    {
        await ConcreteMaps(await LoadMapInfos());
        if (Runner.IsSceneAuthority)
            LoadCompleteRpc();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void LoadCompleteRpc()
    {
        isLoading = false;
        StartCoroutine(DelayGameStart());
    }

    private IEnumerator DelayGameStart()
    {
        yield return new WaitForSeconds(4f);
        EventManager.Instance.OnMapLoadedComplete();
    }

    private async UniTask<List<MapInfo>> LoadMapInfos()
    {
        List<UniTask> tasks = new List<UniTask>();
        List<MapInfo> mapInfos = new List<MapInfo>();
        foreach (var key in maps.Keys)
        {
            if (!playerIds.Contains(key))
                continue;
            if (maps[key].mapType == MapInfo.MapType.Default)
                tasks.Add(LoadDefaultMapInfo(mapInfos, maps[key].mapId));
            else
                tasks.Add(LoadCustomMapInfo(mapInfos, maps[key].mapId));
            Debug.Log($"LoadFindPhotoMap: load map {maps[key].mapId} ({maps[key].mapType})");
        }

        if (tasks.Count < 4)
        {
            for (int i = tasks.Count; i < 4; i++)
            {
                tasks.Add(LoadDefaultMapInfo(mapInfos, i));
                Debug.Log($"LoadFindPhotoMap: load map {i} (default)");
            }
        }
        
        await UniTask.WhenAll(tasks);
        mapInfos.Sort((a, b) =>
        {
            if (a.id == b.id)
            {
                return a.type == MapInfo.MapType.Default ? -1 : 1;
            }
            else
            {
                return a.id.CompareTo(b.id);
            }
            
        });
        
        return mapInfos;
    }

    private async UniTask LoadDefaultMapInfo(List<MapInfo> mapInfos, long mapId)
    {
        mapInfos.Add(await MapManager.Instance.LoadDefaultMapInfoFromServer(mapId));
    }
    private async UniTask LoadCustomMapInfo(List<MapInfo> mapInfos, long mapId)
    {
        mapInfos.Add(await MapManager.Instance.LoadCustomMapInfoFromServer(mapId));
    }

    private async UniTask ConcreteMaps(List<MapInfo> mapInfos)
    {
        Vector3[] pos = new[]
        {
            new Vector3(22.5f, 0f, 22.5f),
            new Vector3(-22.5f, 0f, 22.5f),
            new Vector3(-22.5f, 0f, -22.5f),
            new Vector3(22.5f, 0f, -22.5f)
        };

        for (int i = 0; i < 4; i++)
        {
            MapConcrete mapConcrete = await MapConverter.ConvertMapInfoToMapConcrete(mapInfos[i]);
            mapConcrete.rootObject.transform.position = pos[i];
            LoadWater(mapConcrete).Forget();
        }
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
            water.transform.localPosition = theme.water.position;
        }
    }
}
