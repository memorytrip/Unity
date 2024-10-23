using System.Collections;
using Common.Network;
using Fusion;
using UnityEngine;

public class Testmanager : MonoBehaviour
{
    public NetworkPrefabRef playerPrefab;

    void Start()
    {
        {
            StartCoroutine(PlayerSpawn());
        }
    }

    private IEnumerator PlayerSpawn()
    {
        var op = RunnerManager.Instance.Runner.SpawnAsync(playerPrefab);
        yield return new WaitUntil(() => op.Status == NetworkSpawnStatus.Spawned);
    }
}
