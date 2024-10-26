using Fusion;
using UnityEngine;

public class ConnectMainCamToCanvas : NetworkBehaviour
{
    private Canvas _canvas;
    
    public override void Spawned()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }
}
