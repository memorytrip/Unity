using System.Collections;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Fusion;
using Map;
using UnityEngine;

public class LoadFindPhotoMap : NetworkBehaviour
{
    [SerializeField] private Transform mapsObject;

    [HideInInspector] public bool isLoading;

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
    }

    private async UniTask<List<MapInfo>> LoadMapInfos()
    {
        return await MapManager.Instance.LoadMapList(new User());
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
